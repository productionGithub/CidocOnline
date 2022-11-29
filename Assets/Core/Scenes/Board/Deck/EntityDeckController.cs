#define TRACE_ON
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
    public class EntityDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls card controller and deck related interactables (Slider + Color ticks)
        /// Manage hierarchy display and events
        /// Note : UI deck logic is managed at this controller level, not the manager
        /// </summary>

        [Inject] EntityDeckService _entityDeckService;

        public event Action<float> OnTickFilterDeckController;

        public List<EntityCard> _initialDeckContent;
        public List<EntityCard> _currentDeckContent;
        public List<EntityCard> _addedCard;//Keeps track of cards that does not belong to initial deck

        public EntityCard CurrentCard;

        [SerializeField]
        EntityCardController _entityCardController;//Card
        [SerializeField]
        EntityTicksController _ticksController;
        [SerializeField]
        HierarchyEntityDisplayer _hierarchyDisplayer;//Class hierarchy of card
        [SerializeField]
        DeckSliderController _sliderController;//Slider
        [SerializeField]
        Button _previousStepper;//Stepper -
        [SerializeField]
        Button _nextStepper;// Stepper +
        [SerializeField]
        DeckCounterDisplayer _deckCounterDisplayer;//Deck counter
        [SerializeField]
        GameObject _noMatchCard;//Shows when no card matches filtering

        private bool _isListFiltered = false;
        private bool _initDone = false;

        //Initialization is done once.
        public void Init(List<EntityCard> initialDeck)
        {
            //Initialization is made only once
            if (_initDone == false)
            {
                CurrentCard = new EntityCard();

                //Card data
                _entityCardController.Init();

                //Ticks
                _ticksController.Init();
                _ticksController.OnEntityTickClicked_TicksCtrl += UpdateColorFilters;

                //Steppers
                _previousStepper.onClick.AddListener(OnPreviousCardClicked);
                _nextStepper.onClick.AddListener(OnNextCardClicked);

                //HierarchyDisplayer
                _hierarchyDisplayer.HierarchyEntityEntryClickEvent += OnHierarchyEntityClick;

                //Slider
                _sliderController.Init();
                _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

                _initDone = true;
            }
        }

        public void InitDeck(List<EntityCard> initialDeck)
        {
            _initialDeckContent = new List<EntityCard>(initialDeck);
            _currentDeckContent = new List<EntityCard>(initialDeck);
            _addedCard = new List<EntityCard>();
            CurrentCard = initialDeck[0];
        }

        public void Show()
        {
            ReinitDeck();//No filtering + ticks are all off but the white one

            _entityCardController.Show(_initialDeckContent[0]);
            _hierarchyDisplayer.Init();//Hierarachy is destroyer and recreated for each card
            _hierarchyDisplayer.Show(_initialDeckContent[0]);
            _sliderController.Show(_currentDeckContent.Count - 1);
            _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);

            _noMatchCard.SetActive(false);
        }

        private void OnHierarchyEntityClick(string cardId)
        {
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == cardId);
            DisplayCardFromHierarchy(card);
        }

        public void DisplayCardFromHierarchy(EntityCard card)
        {
            //If newCard exists, just Refresh the card displayer with it
            if (_currentDeckContent.Exists(x => x.id.Equals(card.id)))
            {
                int index = _currentDeckContent.FindIndex(c => c.id == card.id);
                _sliderController.SetSliderValue(index);//Will fire a 'Refresh card' event
            }
            else //If newCard does not exist add it to the initial deck
            {
                _addedCard.Add(card);//It is not part of initial deck so add it to this list

                _currentDeckContent.Add(card);//add new card in deck
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);
                _sliderController.SetSliderValue(_currentDeckContent.Count - 1);


                _deckCounterDisplayer.Show(_currentDeckContent.Count, _currentDeckContent.Count);
            }
        }

        private void OnSliderValueChanged(float value)
        {
            if (value < _currentDeckContent.Count - 1 || value > 0)
            {
                EntityCard currentCard = _currentDeckContent[(int)value];

                //Refresh card
                _entityCardController.Show(currentCard);
                GhostCardIfExists((int)value);

                //Display hierarchy
                _hierarchyDisplayer.Init();
                _hierarchyDisplayer.Show(currentCard);

                //Update player answer
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

        private void GhostCardIfExists(int index)
        {
            if (_addedCard.Exists(x => x.id.Equals(_currentDeckContent[index].id)))
            {
                _entityCardController.GhostBackground();
            }
            else
            {
                _entityCardController.ReinitBackground();
            }
        }

        public void UpdateColorFilters(GameObject sender, EntityTick.TickColor color)
        {
            Trace.Log("WE UPDATE COLOR WITH : " + sender.name + " " + color.ToString());
            _noMatchCard.SetActive(false);
            _sliderController.SetSliderActive(true);

            //If white selected, re-init with initial deck
            if (color == EntityTick.TickColor.White)
            {
                ReinitDeck();
            }
            else
            {
                //If deck is not already filtered, clear it.
                //If already filtered, color filtering is cumulative
                if (_isListFiltered == false)
                {
                    _currentDeckContent.Clear();
                }

                //Add corresponding tick color to filter
                if (sender.GetComponent<EntityTick>().IsTicked == true)
                {
                    FilterAddColor(color);
                    if (_currentDeckContent.Count > 0)
                    {
                        _isListFiltered = true;
                    }
                }
                else
                //Remove corresponding tick color to filter
                {
                    FilterRemoveColor(color);
                    _isListFiltered = true;

                    //If tick is the last one to be ticked, reset deck to initial
                    if (_ticksController.TickCount <= 0)
                    {
                        _currentDeckContent.Clear();
                        _currentDeckContent = new List<EntityCard>(_initialDeckContent);
                        _addedCard.Clear();
                        _addedCard = new List<EntityCard>();
                        _isListFiltered = false;
                    }
                }
            }
            CurrentCard = _currentDeckContent[0];
            DisplayDeck();
        }

        private void FilterAddColor(EntityTick.TickColor e)//From current deckContent
        {
            foreach (EntityCard curCard in _initialDeckContent)
            {
                if (curCard.colors[0].Equals((Color32)_entityDeckService.ColorsDictionary[e.ToString()]))
                {
                    _currentDeckContent.Add(curCard);
                }
                else
                {
                    if (curCard.colors.Count > 1)
                    {
                        if (curCard.colors[1].Equals((Color32)_entityDeckService.ColorsDictionary[e.ToString()]))
                        {
                            _currentDeckContent.Add(curCard);
                        }
                    }
                }
            }
        }

        private void FilterRemoveColor(EntityTick.TickColor e)
        {
            foreach (EntityCard curCard in _initialDeckContent)
            {
                //EntityCard curCard = decksCtrl.entityCards[id];
                if (curCard.colors[0].Equals((Color32)_entityDeckService.ColorsDictionary[e.ToString()]))
                {
                    _currentDeckContent.Remove(curCard);
                }
                else
                {
                    if (curCard.colors.Count > 1)
                    {
                        if (curCard.colors[1].Equals((Color32)_entityDeckService.ColorsDictionary[e.ToString()]))
                        {
                            _currentDeckContent.Remove(curCard);
                        }
                    }
                }
            }
        }

        private void DisplayDeck()
        {
            if (_currentDeckContent.Count > 0)//If deckcontains at least one card, refresh it
            {
                _noMatchCard.SetActive(false);

                _entityCardController.Show(_currentDeckContent[0]);
                _entityCardController.ReinitBackground();

                _hierarchyDisplayer.Init();
                _hierarchyDisplayer.Show(_currentDeckContent[0]);

                _sliderController.SetSliderActive(true);
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);
                _sliderController.SetSliderValue(0);

                _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);
            }
            else//If not, display 'No match' card and update deck counter
            {
                _noMatchCard.SetActive(true);
                //hide full text
                _entityCardController.ReInitFullText();
                _sliderController.SetSliderActive(false);
                _deckCounterDisplayer.Show(0, _initialDeckContent.Count);
            }
        }

        private void ReinitDeck()
        {
            _isListFiltered = false;

            _currentDeckContent.Clear();
            _currentDeckContent = new List<EntityCard>(_initialDeckContent);

            _addedCard.Clear();//Maybe will be remove depending on chosen logic
            _addedCard = new List<EntityCard>();

            _ticksController.ResetTicks();
        }

        private void OnDestroy()
        {
            _ticksController.OnEntityTickClicked_TicksCtrl -= UpdateColorFilters;
            _sliderController.OnSliderValueChangedUI -= OnSliderValueChanged;
            _previousStepper.onClick.RemoveListener(OnPreviousCardClicked);
            _nextStepper.onClick.RemoveListener(OnNextCardClicked);
            _hierarchyDisplayer.HierarchyEntityEntryClickEvent -= OnHierarchyEntityClick;
        }
    }
}