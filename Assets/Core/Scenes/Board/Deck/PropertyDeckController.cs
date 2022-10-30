using StarterCore.Core.Scenes.Board.Card.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Deck.DeckInteractables;
using StarterCore.Core.Scenes.Board.Displayer;
using TMPro;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class PropertyDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls deck displayers
        /// Controls deck interactables (Slider + Ticks)
        /// Note : UI deck logic is managed at this controller level, not the manager
        /// </summary>
        ///
        [Inject] EntityDeckService _entityDeckService;
        [Inject] PropertyDeckService _propertyDeckService;

        public event Action<float> OnTickFilterDeckController;

        public List<PropertyCard> _initialDeckContent;
        public List<PropertyCard> _currentDeckContent;
        public List<PropertyCard> _addedCard;//Keeps track of cards that does not belong to initial deck

        [SerializeField]
        PropertyCardDisplayer _propertyCardDisplayer;//Card
        [SerializeField]
        HierarchyPropertyDisplayer _hierarchyDisplayer;//Class hierarchy of card
        [SerializeField]
        DeckSliderController _sliderController;//Slider
        [SerializeField]
        Button _previousStepper;//Stepper -
        [SerializeField]
        Button _nextStepper;// Stepper +
        [SerializeField]
        TicksController _domainTickController;
        [SerializeField]
        TicksController _rangeTickController;
        [SerializeField]
        GameObject _noMatchCard;
        [SerializeField]
        DeckCounterDisplayer _deckCounterDisplayer;//Deck counter

        //Ref to decks controllers for Domain and Range buttons
        [SerializeField]
        EntityDeckController _leftDeckController;
        [SerializeField]
        EntityDeckController _rightDeckController;

        private bool isListFiltered = false;

        //Update color filter
        private List<string> domainColorFilter;
        private List<string> rangeColorFilter;
        private bool isDomainColorListWhite = false;
        private bool isRangeColorListWhite = false;

        public GameObject domainTicksContainer;
        public GameObject rangeTicksContainer;


        public void Show(List<PropertyCard> initialDeck)
        {
            domainColorFilter = new List<string>();
            rangeColorFilter = new List<string>();
            InitColorList(ref domainColorFilter);
            InitColorList(ref rangeColorFilter);

            _initialDeckContent = initialDeck;
            _addedCard = new List<PropertyCard>();
            _currentDeckContent = new List<PropertyCard>(initialDeck);

            _propertyCardDisplayer.Show(initialDeck[0]);

            _hierarchyDisplayer.Show(initialDeck[0]);
            _hierarchyDisplayer.HierarchyPropDisp_HierarchyClickEvent += OnHierarchyEntityClick;

            _propertyCardDisplayer.OnDomainButtonClick += OnDomainButtonClicked;
            _propertyCardDisplayer.OnRangeButtonClick += OnRangeButtonClicked;

            _sliderController.Show(_currentDeckContent.Count - 1);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

            _previousStepper.onClick.AddListener(OnPreviousCardClicked);
            _nextStepper.onClick.AddListener(OnNextCardClicked);

            _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);

            _noMatchCard.SetActive(false);
        }

        private void OnHierarchyEntityClick(string cardId)
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
                _propertyCardDisplayer.Refresh(_currentDeckContent[index]);
                _sliderController.SetSliderValue(index);
            }

            else //If newCard does not exist add it to the initial deck
            {
                _addedCard.Add(card);//It is not part of initial deck so add it to this list
                _currentDeckContent.Add(card);//add new card in deck
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);//Deck has 1 more card, update slider range

                _propertyCardDisplayer.Refresh(_currentDeckContent[_currentDeckContent.Count - 1]);

                _sliderController.SetSliderValue(_currentDeckContent.Count - 1);//Set slider to 'the end' of deck (=== size of deck)
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _currentDeckContent.Count);
            }
        }

        private void OnSliderValueChanged(float value)
        {
            if (value < _currentDeckContent.Count - 1 || value > 0)//Will be currentDeckContent
            {
                //Refresh card
                _propertyCardDisplayer.Refresh(_currentDeckContent[(int)value]);
                GhostCardIfExists((int)value);
                //Refresh hierarchy
                _hierarchyDisplayer.Show(_currentDeckContent[(int)value]);
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
            Trace.Log("[PropertyDeckDisplayer] Domain Butt clicked ");
            string id = cardAbout.Substring(0, cardAbout.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == id);
            _leftDeckController.DisplayCardFromHierarchy(card);
        }

        private void OnRangeButtonClicked(string cardAbout)
        {
            Trace.Log("[PropertyDeckDisplayer] Range Butt clicked ");
            string id = cardAbout.Substring(0, cardAbout.IndexOf("_")).Trim();//Fetch the sub string before first '_'
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == id);
            _rightDeckController.DisplayCardFromHierarchy(card);
        }

        private void GhostCardIfExists(int index)
        {
            if (_addedCard.Exists(x => x.id.Equals(_currentDeckContent[index].id)))
            {
                _propertyCardDisplayer.GhostBackground();
            }
            else
            {
                _propertyCardDisplayer.ReinitBackground();
            }
        }

        //Update domain and range colors list based on tick type
        public void UpdateColorFilters(GameObject sender, TickCtrl.TickColor e)
        {
            Trace.Log("PROPERTY UPDATE COLOR FILTER");
            //In case the previous matching failed:
            //Hide the 'NoMatchCard' and re-activate the slider
            _noMatchCard.SetActive(false);
            _sliderController.SetSliderActive(true);

            switch (sender.GetComponent<TickCtrl>().typeOfTick)
            {
                case (TickCtrl.TickType.Domain):
                    if (e == TickCtrl.TickColor.White)
                    {
                        //White has a special behavior:
                        //If not ticked, this means that at least one other color is ticked.
                        //See TicksCtrl.RefreshTick() for detail.
                        if (sender.GetComponent<TickCtrl>().IsTicked == true)//Tick just passed to 'On'
                        {
                            //Remove all previous selected colors
                            InitDomainColors();
                        }
                    }
                    else
                    {
                        if (sender.GetComponent<TickCtrl>().IsTicked == true)
                        {
                            //If color list is not already filtered, clear it.
                            //If already filtered, color filtering is cumulative
                            if (isDomainColorListWhite == true)
                            {
                                domainColorFilter.Clear();
                            }
                            domainColorFilter.Add((string)e.ToString());
                            isDomainColorListWhite = false;
                        }
                        else
                        //Tick is Off => Remove color
                        {
                            domainColorFilter.Remove(e.ToString());
                            //If no color anymore in the list, re-init to white (All colors)
                            if (domainTicksContainer.GetComponent<TicksController>().TickCount == 0)
                            {
                                InitColorList(ref domainColorFilter);
                                isDomainColorListWhite = true;
                            }
                        }
                    }
                    break;

                case (TickCtrl.TickType.Range):
                    if (e == TickCtrl.TickColor.White)
                    {
                        //White has a special behavior:
                        //If not ticked, this means that at least one other color is ticked.
                        //See TicksCtrl.RefreshTick() for detail.
                        if (sender.GetComponent<TickCtrl>().IsTicked == true)//Tick just passed to 'On'
                        {
                            //Remove all previous color
                            InitRangeColors();
                        }
                    }
                    else
                        if (sender.GetComponent<TickCtrl>().IsTicked == true)
                    {
                        //If color list is not already filtered, clear it.
                        //If already filtered, color filtering is cumulative
                        if (isRangeColorListWhite == true)
                        {
                            rangeColorFilter.Clear();
                        }
                        rangeColorFilter.Add(e.ToString());
                        isRangeColorListWhite = false;
                    }
                    else
                    //Tick is Off => Remove color
                    {
                        rangeColorFilter.Remove(e.ToString());
                        //If no color anymore in the list, re-init to white (All colors)
                        if (rangeTicksContainer.GetComponent<TicksController>().TickCount == 0)
                        {
                            InitColorList(ref rangeColorFilter);
                            isRangeColorListWhite = true;
                        }
                    }
                    break;

                default:
                    break;
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
            UpdateDeck();
            domainTicksContainer.GetComponent<TicksController>().ResetTicks();
            rangeTicksContainer.GetComponent<TicksController>().ResetTicks();
        }

        private void UpdateDeck()
        {
            Trace.Log("PROPERTY UPDATE DECK !");
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
                _noMatchCard.SetActive(false);
                foreach (PropertyCard card in cards)
                {
                    _currentDeckContent.Add(card);
                }
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);

                //Update deck content info
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);
                _propertyCardDisplayer.Refresh(_propertyDeckService.PropertyCards[0]);
            }
            else
            {
                _noMatchCard.SetActive(true);
                _sliderController.SetSliderActive(false);
                _deckCounterDisplayer.Show(0, _initialDeckContent.Count);
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
    }
}