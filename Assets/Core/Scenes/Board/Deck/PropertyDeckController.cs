#define TRACE_OFF
using StarterCore.Core.Scenes.Board.Card.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Deck.DeckInteractables;
using StarterCore.Core.Scenes.Board.Displayer;
using StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class PropertyDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls deck controllers
        /// Controls deck interactables (Slider + Ticks + hierarchy)
        /// Note : UI deck logic is managed at this controller level, not the manager
        /// </summary>
        
        [Inject] readonly EntityDeckService _entityDeckService;
        [Inject] readonly PropertyDeckService _propertyDeckService;

        public event Action<float> OnTickFilterDeckController;

        public List<PropertyCard> _initialDeckContent;
        public List<PropertyCard> _currentDeckContent;
        public List<PropertyCard> _addedCard;//Keeps track of cards that does not belong to initial deck

        public PropertyCard CurrentCard;

        [SerializeField] PropertyCardController _propertyCardController;//Card
        [SerializeField] PropertyTicksController _domainTicksController;
        [SerializeField] PropertyTicksController _rangeTicksController;
        [SerializeField] HierarchyPropertyDisplayer _hierarchyDisplayer;//Class hierarchy of card
        [SerializeField] DeckSliderController _sliderController;//Slider
        [SerializeField] Button _previousStepper;//Stepper -
        [SerializeField] Button _nextStepper;// Stepper +
        [SerializeField] GameObject _noMatchCard;
        [SerializeField] DeckCounterDisplayer _deckCounterDisplayer;//Deck counter

        //Ref to decks controllers for Domain and Range buttons
        [SerializeField] EntityDeckController _leftDeckController;
        [SerializeField] EntityDeckController _rightDeckController;

        //Update color filter
        private List<string> domainColorFilter;
        private List<string> rangeColorFilter;

        private bool isDomainColorListWhite;
        private bool isRangeColorListWhite;

        private bool _initDone = false;

        public void Init(List<PropertyCard> initialDeck)
        {
            if (_initDone == false)
            {
                //Card
                _propertyCardController.Init();

                //Ticks
                _domainTicksController.Init();
                _domainTicksController.OnPropertyTickClicked_TicksCtrl += UpdateColorFilters;

                _rangeTicksController.Init();
                _rangeTicksController.OnPropertyTickClicked_TicksCtrl += UpdateColorFilters;

                //hierarchy
                _hierarchyDisplayer.HierarchyPropertyEntryClickEvent += OnHierarchyPropertyClick;

                _propertyCardController.OnDomainButtonClick_Controller += OnDomainButtonClicked;
                _propertyCardController.OnRangeButtonClick_Controller += OnRangeButtonClicked;

                //Slider
                _sliderController.Init();
                _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

                //Steppers
                _nextStepper.onClick.AddListener(OnNextCardClicked);
                _previousStepper.onClick.AddListener(OnPreviousCardClicked);

                isDomainColorListWhite = true;
                isRangeColorListWhite = true;

                _initDone = true;
            }
        }

        public void InitDeck(List<PropertyCard> initialDeck)
        {
            _initialDeckContent = new List<PropertyCard>(initialDeck);
            CurrentCard = initialDeck[0];
            Trace.Log("[PropertyDeckController] Init!");

        }
        public void Show()
        {
            domainColorFilter = new List<string>();
            rangeColorFilter = new List<string>();

            //Read colors of card
            InitColorList(ref domainColorFilter);
            InitColorList(ref rangeColorFilter);

            isDomainColorListWhite = true;//By default, white tick / all colors are enabled
            isRangeColorListWhite = true;//By default, white tick / all colors are enabled

            _addedCard = new List<PropertyCard>();
            _currentDeckContent = new List<PropertyCard>(_initialDeckContent);

            _propertyCardController.Show(_initialDeckContent[0]);

            _domainTicksController.ResetTicks();
            _rangeTicksController.ResetTicks();

            _hierarchyDisplayer.Init();
            _hierarchyDisplayer.Show(_initialDeckContent[0]);

            _sliderController.Show(_currentDeckContent.Count - 1);
            _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);

            _noMatchCard.SetActive(false);
        }

        private void OnHierarchyPropertyClick(string cardId)
        {
            PropertyCard card = _propertyDeckService.PropertyCards.Single(c => c.id == cardId);
            DisplayCard(card);
        }

        private void DisplayCard(PropertyCard card)
        {
            //If newCard exists, just Refresh the card displayer with it
            if (_currentDeckContent.Exists(x => x.id.Equals(card.id)))
            {
                int index = _currentDeckContent.FindIndex(c => c.id == card.id);
                //_propertyCardDisplayer.Show(_currentDeckContent[index]);
                _propertyCardController.Show(_currentDeckContent[index]);
                _sliderController.SetSliderValue(index);
            }

            else //If newCard does not exist add it to the initial deck
            {
                _addedCard.Add(card);//It is not part of initial deck so add it to this list
                _currentDeckContent.Add(card);//add new card in deck
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);//Deck has 1 more card, update slider range

                _propertyCardController.Show(_currentDeckContent[_currentDeckContent.Count - 1]);

                _sliderController.SetSliderValue(_currentDeckContent.Count - 1);//Set slider to 'the end' of deck (=== size of deck)
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _currentDeckContent.Count);
            }
        }

        private void OnSliderValueChanged(float value)
        {
            if (value < _currentDeckContent.Count - 1 || value > 0)//Will be currentDeckContent
            {
                PropertyCard currentCard = _currentDeckContent[(int)value];
                //Refresh card
                _propertyCardController.Show(currentCard);

                GhostCardIfExists((int)value);

                //Refresh hierarchy
                _hierarchyDisplayer.Init();
                _hierarchyDisplayer.Show(currentCard);

                CurrentCard = currentCard;
            }
        }

        private void OnPreviousCardClicked()
        {
            if (_sliderController.GetComponent<Slider>().value > 0)
            {
                float previousCardIndex = _sliderController.GetComponent<Slider>().value - 1;
                _sliderController.SetSliderValue(previousCardIndex);//Will refresh and ghost card if needed
            }
        }

        private void OnNextCardClicked()
        {
            if (_sliderController.GetComponent<Slider>().value < _currentDeckContent.Count - 1)
            {
                float nextCardIndex = _sliderController.GetComponent<Slider>().value + 1;
                _sliderController.SetSliderValue(nextCardIndex);//Will refresh and ghost card if needed
            }
        }

        private void OnDomainButtonClicked(string cardAbout)
        {
            string id = cardAbout.Substring(0, cardAbout.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == id);
            _leftDeckController.DisplayCardFromHierarchy(card);
        }

        private void OnRangeButtonClicked(string cardAbout)
        {
            string id = cardAbout.Substring(0, cardAbout.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == id);
            _rightDeckController.DisplayCardFromHierarchy(card);
        }

        private void GhostCardIfExists(int index)
        {
            if (_addedCard.Exists(x => x.id.Equals(_currentDeckContent[index].id)))
            {
                _propertyCardController.GhostBackground();
            }
            else
            {
                _propertyCardController.ReinitBackground();
            }
        }

        //Update domain and range colors list based on tick type
        public void UpdateColorFilters(GameObject sender, PropertyTick.TickColor color)
        {
            Trace.Log(string.Format("[UPDATE COLOR FILTER] Received color :", color));

            //In case the previous matching failed:
            //Hide the 'NoMatchCard' and re-activate the slider
            _noMatchCard.SetActive(false);
            _sliderController.SetSliderActive(true);

            switch (sender.GetComponent<PropertyTick>().TypeOfTick)
            {
                case (PropertyTick.TickType.Domain):
                    if (color == PropertyTick.TickColor.White)
                    {
                        //White has a special behavior:
                        //If not ticked, this means that at least one other color is ticked.
                        if (sender.GetComponent<PropertyTick>().IsTicked == true)//Tick just passed to 'On'
                        {
                            //Remove all previous selected colors
                            InitDomainColors();
                        }
                    }
                    else
                    {
                        if (sender.GetComponent<PropertyTick>().IsTicked == true)
                        {
                            //If color list is not already filtered, clear it.
                            //If already filtered, color filtering is cumulative
                            if (isDomainColorListWhite == true)
                            {
                                domainColorFilter.Clear();
                            }
                            domainColorFilter.Add((string) color.ToString());
                            Debug.Log("");
                            isDomainColorListWhite = false;
                        }
                        else
                        //Tick is Off => Remove color
                        {
                            domainColorFilter.Remove(color.ToString());
                            //If no color anymore in the list, re-init to white (All colors)
                            if (_domainTicksController.TickCount == 0)
                            {
                                InitColorList(ref domainColorFilter);
                                isDomainColorListWhite = true;
                            }
                        }
                    }
                    break;

                case (PropertyTick.TickType.Range):
                    if (color == PropertyTick.TickColor.White)
                    {
                        //White has a special behavior:
                        //If not ticked, this means that at least one other color is ticked.
                        if (sender.GetComponent<PropertyTick>().IsTicked == true)//Tick just passed to 'On'
                        {
                            //Remove all previous color
                            InitRangeColors();
                        }
                    }
                    else
                    {
                        if (sender.GetComponent<PropertyTick>().IsTicked == true)
                        {
                            //If color list is not already filtered, clear it.
                            //If already filtered, color filtering is cumulative
                            if (isRangeColorListWhite == true)
                            {
                                rangeColorFilter.Clear();
                            }
                            rangeColorFilter.Add(color.ToString());
                            isRangeColorListWhite = false;
                        }
                        else
                        //Tick is Off => Remove color
                        {
                            rangeColorFilter.Remove(color.ToString());
                            //If no color anymore in the list, re-init to white (All colors)
                            if (_rangeTicksController.TickCount == 0)//rangeTicksContainer.GetComponent<PropertyTicksController>().TickCount == 0
                            {
                                InitColorList(ref rangeColorFilter);
                                isRangeColorListWhite = true;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
            Trace.Log("AFTER FILTERING : Domain color list =>");
            foreach(string c in domainColorFilter)
            {
                Trace.Log(string.Format("domain color : {0} ", c));
            }
            Trace.Log("AFTER FILTERING : Range color list =>");
            foreach (string c in rangeColorFilter)
            {
                Trace.Log(string.Format("domain color : {0} ", c));
            }
            UpdateDeck();
        }

        private void InitDomainColors()
        {
            InitColorList(ref domainColorFilter);
            isDomainColorListWhite = true;
        }

        private void InitRangeColors()
        {
            InitColorList(ref rangeColorFilter);
            isRangeColorListWhite = true;
        }

        public void ResetDeck()
        {
            InitDomainColors();
            InitRangeColors();

            _currentDeckContent.Clear();
            _addedCard.Clear();
            _currentDeckContent = new List<PropertyCard>(_initialDeckContent);
            _addedCard = new List<PropertyCard>();

            UpdateDeck();

            _propertyCardController.Show(_propertyDeckService.PropertyCards[0]);

            _domainTicksController.ResetTicks();
            _rangeTicksController.ResetTicks();
        }

        private void UpdateDeck()
        {
            _currentDeckContent.Clear();

            var dcf = domainColorFilter;
            var rcf = rangeColorFilter;

            IEnumerable<PropertyCard> cards =
                        from card in _initialDeckContent
                        let dc = card.domainColors
                        let rc = card.rangeColors
                        where dc.Intersect(dcf).Any() && rc.Intersect(rcf).Any()
                        select card;

            if (cards.Count() > 0)
            {
                Trace.Log("[PropertyController] MIN 1 card after filtering.");
                _noMatchCard.SetActive(false);
                foreach (PropertyCard card in cards)
                {
                    _currentDeckContent.Add(card);
                }

                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);

                //Update deck content info
                Trace.Log("[PropertyDeckController] UPDATE DECK COUNTER with value : " + _currentDeckContent.Count);
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);
                Trace.Log("[PropertyDeckController] CAll refresh card: ");
                _propertyCardController.Show(_propertyDeckService.PropertyCards[0]);
            }
            else
            {
                Trace.Log("[PropertyController] NO card after filtering.");
                Trace.Log("MATCH NO CARD");
                _noMatchCard.SetActive(true);
                _sliderController.SetSliderActive(false);
                _deckCounterDisplayer.Show(0, _initialDeckContent.Count);
                //Hide FullText
                _propertyCardController.ReInitFullText();
            }
        }

        private void InitColorList(ref List<string> list)
        {
            //Remove all previous colors
            list.Clear();
            //White = All colors => Add all colors to domain color list
            foreach (KeyValuePair<string, Color32> color in _propertyDeckService.ColorsDictionary)
            {
                list.Add(color.Key.ToString());
            }
        }

        private void OnDestroy()
        {
            _domainTicksController.OnPropertyTickClicked_TicksCtrl -= UpdateColorFilters;
            _sliderController.OnSliderValueChangedUI -= OnSliderValueChanged;
            _nextStepper.onClick.RemoveListener(OnNextCardClicked);
            _previousStepper.onClick.RemoveListener(OnPreviousCardClicked);
            _propertyCardController.OnDomainButtonClick_Controller -= OnDomainButtonClicked;
            _propertyCardController.OnRangeButtonClick_Controller -= OnRangeButtonClicked;
        }
    }
}