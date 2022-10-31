using StarterCore.Core.Scenes.Board.Card.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Deck.DeckInteractables;
using StarterCore.Core.Scenes.Board.Displayer;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class EntityDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls deck displayers
        /// Controls deck interactables (Slider + Ticks)
        /// Note : UI deck logic is managed at this controller level, not the manager
        /// </summary>
        /// 
        [Inject] EntityDeckService _entityDeckService;

        public event Action<float> OnTickFilterDeckController;

        public List<EntityCard> _initialDeckContent;
        public List<EntityCard> _currentDeckContent;
        public List<EntityCard> _addedCard;//Keeps track of cards that does not belong to initial deck

        [SerializeField]
        EntityCardDisplayer _entityCardDisplayer;//Card
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
        [SerializeField] TicksController _tickController;
        [SerializeField] GameObject _noMatchCard;

        //[SerializeField] TicksController _ticksContainer;

        private bool isListFiltered = false;


        //TODO CALL it from parent!
        public void InitListeners()
        {
            //EntityCardDisplayer
            //_entityCardDisplayer.InitListeners();


            //Slider
            //_sliderController.InitSliderListener();
            //_sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

            //HierarchyDisplayer
            _hierarchyDisplayer.HierarchyEntDisp_HierarchyClickEvent += OnHierarchyEntityClick;
        }

        public void Show(List<EntityCard> initialDeck)
        {
            _initialDeckContent = initialDeck;
            _addedCard = new List<EntityCard>();
            _currentDeckContent = new List<EntityCard>(initialDeck);


            _entityCardDisplayer.Show(initialDeck[0]);

            _hierarchyDisplayer.Show(initialDeck[0]);
            _hierarchyDisplayer.HierarchyEntDisp_HierarchyClickEvent += OnHierarchyEntityClick;

            //Steppers
            _previousStepper.onClick.AddListener(OnPreviousCardClicked);
            _nextStepper.onClick.AddListener(OnNextCardClicked);

            //Slider
            _sliderController.Show(_currentDeckContent.Count - 1);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

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

                //_entityCardDisplayer.Refresh(_currentDeckContent[index]);
                _entityCardDisplayer.Refresh(card);
                int index = _currentDeckContent.FindIndex(c => c.id == card.id);
                _sliderController.SetSliderValue(index);
            }

            else //If newCard does not exist add it to the initial deck
            {
                _addedCard.Add(card);//It is not part of initial deck so add it to this list

                _currentDeckContent.Add(card);//add new card in deck
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);//Deck has 1 more card, update slider range

                _entityCardDisplayer.Refresh(_currentDeckContent[_currentDeckContent.Count - 1]);

                _sliderController.SetSliderValue(_currentDeckContent.Count - 1);//Set slider to 'the end' of deck (=== size of deck)
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _currentDeckContent.Count);
                
            }
        }

        private void OnSliderValueChanged(float value)
        {
            if (value < _currentDeckContent.Count - 1 || value > 0)//Will be currentDeckContent
            {
                //Refresh card
                _entityCardDisplayer.Refresh(_currentDeckContent[(int)value]);
                //_entityCardController.Refresh(_currentDeckContent[(int)value]);
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

        private void GhostCardIfExists(int index)
        {
            if (_addedCard.Exists(x => x.id.Equals(_currentDeckContent[index].id)))
            {
                _entityCardDisplayer.GhostBackground();
            }
            else
            {
                _entityCardDisplayer.ReinitBackground();
            }
        }

        /*************************************/

        public void UpdateColorFilters(GameObject sender, TickCtrl.TickColor e)
        {
            _noMatchCard.SetActive(false);
            _sliderController.SetSliderActive(true);

            //If white selected, re-init with initial deck
            if (e == TickCtrl.TickColor.White)
            {
                ReinitDeck();
            }
            else
            {
                //If deck is not already filtered, clear it.
                //If already filtered, color filtering is cumulative
                if (isListFiltered == false)
                {
                    _currentDeckContent.Clear();
                }

                //Add corresponding tick color to filter
                if (sender.GetComponent<TickCtrl>().IsTicked == true)
                {
                    FilterAddColor(e);
                    if (_currentDeckContent.Count > 0)
                    {
                        isListFiltered = true;
                    }
                }
                else
                //Remove corresponding tick color to filter
                {
                    FilterRemoveColor(e);
                    isListFiltered = true;

                    //If tick is the last one to be ticked, reset deck to initial
                    if (_tickController.TickCount <= 0)
                    {
                        _currentDeckContent.Clear();
                        _currentDeckContent = new List<EntityCard>(_initialDeckContent);
                        _addedCard.Clear();
                        _addedCard = new List<EntityCard>();
                        isListFiltered = false;
                    }
                }
            }

            DisplayDeck();
        }


        private void FilterAddColor(TickCtrl.TickColor e)//From current deckContent
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


        private void FilterRemoveColor(TickCtrl.TickColor e)
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

        private void ReinitDeck()
        {
            //Show(_initialDeckContent);//Reinit initial deck
            _currentDeckContent.Clear();
            _addedCard.Clear();//Maybe will be remove depending on chosen logic
            _currentDeckContent = new List<EntityCard>(_initialDeckContent);
            _addedCard = new List<EntityCard>();

            isListFiltered = false;
        }

        private void DisplayDeck()
        {
            if (_currentDeckContent.Count > 0)//If deckcontains at least one card after filtering,
            {
                _noMatchCard.SetActive(false);
                _entityCardDisplayer.Refresh(_currentDeckContent[0]);
                _entityCardDisplayer.ReinitBackground();

                _hierarchyDisplayer.Show(_currentDeckContent[0]);

                _sliderController.SetSliderActive(true);
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);
                _sliderController.SetSliderValue(0);

                _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);
            }
            else//If not, display 'No match' card and update deck counter
            {
                _noMatchCard.SetActive(true);
                _sliderController.SetSliderActive(false);
                _deckCounterDisplayer.Show(0, _initialDeckContent.Count);
            }
        }

        public void ResetDeck()
        {
            ReinitDeck();
            DisplayDeck();
            _entityCardDisplayer.ResetToFirstCard();
            _tickController.ResetTicks();
        }
    }
}