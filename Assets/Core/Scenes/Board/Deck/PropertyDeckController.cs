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
    public class PropertyDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls deck displayers
        /// Controls deck interactables (Slider + Ticks)
        /// Note : UI deck logic is managed at this controller level, not the manager
        /// </summary>
        /// 
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
        TicksCtrl _domainTickController;
        [SerializeField]
        TicksCtrl _rangeTickController;
        [SerializeField]
        GameObject _noMatchCard;
        [SerializeField]
        DeckCounterDisplayer _deckCounterDisplayer;//Deck counter

        private bool isListFiltered = false;

        public void Show(List<PropertyCard> initialDeck)
        {
            _initialDeckContent = initialDeck;
            _addedCard = new List<PropertyCard>();
            _currentDeckContent = new List<PropertyCard>(initialDeck);

            _propertyCardDisplayer.Show(initialDeck[0]);

            _hierarchyDisplayer.Show(initialDeck[0]);
            _hierarchyDisplayer.HierarchyPropDisp_HierarchyClickEvent += OnHierarchyEntityClick;

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
                _propertyCardDisplayer.GhostBackground();
            }
            else
            {
                _propertyCardDisplayer.ReinitBackground();
            }
        }

        /*************************************/

        
        public void UpdateColorFilters(GameObject sender, TickCtrl.TickColor e)
        {
            Debug.Log(string.Format("[EntityDeckController] Receive tick event from {0} with color {1}", sender.name, e));

            _noMatchCard.SetActive(false);
            _sliderController.SetSliderActive(true);

            //If white selected, re-init with initial deck
            if (e == TickCtrl.TickColor.White)
            {
                _currentDeckContent.Clear();
                _addedCard.Clear();//Maybe will be remove depending on chosen logic
                _currentDeckContent = new List<PropertyCard>(_initialDeckContent);
                _addedCard = new List<PropertyCard>();

                isListFiltered = false;
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
                    if (_domainTickController.TickCount <= 0)
                    {
                        _currentDeckContent.Clear();
                        _addedCard.Clear();
                        _currentDeckContent = new List<PropertyCard>(_initialDeckContent);
                        _addedCard = new List<PropertyCard>();
                        isListFiltered = false;
                    }
                }
            }

            if (_currentDeckContent.Count > 0)
            {
                //Update deck content info
                _deckCounterDisplayer.Show(_currentDeckContent.Count, _initialDeckContent.Count);

                _sliderController.SetSliderActive(true);
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);

                if (_currentDeckContent.Count > 1)
                {
                    //ShowDeckAnimation();
                }
                else
                {
                    //HideDeckAnimation();
                }

                _sliderController.SetSliderValue(0);
            }
            else
            {
                //HideDeckAnimation();
                _noMatchCard.SetActive(true);
                _sliderController.SetSliderActive(false);
                _deckCounterDisplayer.Show(0, _initialDeckContent.Count);
            }
        }


        private void FilterAddColor(TickCtrl.TickColor e)//From current deckContent
        {
            /*
            Debug.Log(string.Format("[EntityDeckController] FilterAdd with color : {0}", e));
            foreach (PropertyCard curCard in _initialDeckContent)
            {
                if (curCard.colors[0].Equals((Color32)_propertyDeckService.ColorsDictionary[e.ToString()]))
                {
                    _currentDeckContent.Add(curCard);
                }
                else
                {
                    if (curCard.colors.Count > 1)
                    {
                        if (curCard.colors[1].Equals((Color32)_propertyDeckService.ColorsDictionary[e.ToString()]))
                        {
                            _currentDeckContent.Add(curCard);
                        }
                    }
                }
            }
            */
        }


        private void FilterRemoveColor(TickCtrl.TickColor e)
        {
            /*
            Debug.Log(string.Format("[EntityDeckController] FilterRemove with color : {0}", e));
            foreach (PropertyCard curCard in _initialDeckContent)
            {
                //EntityCard curCard = decksCtrl.entityCards[id];
                if (curCard.colors[0].Equals((Color32)_propertyDeckService.ColorsDictionary[e.ToString()]))
                {
                    _currentDeckContent.Remove(curCard);
                }
                else
                {
                    if (curCard.colors.Count > 1)
                    {
                        if (curCard.colors[1].Equals((Color32)_propertyDeckService.ColorsDictionary[e.ToString()]))
                        {
                            _currentDeckContent.Remove(curCard);
                        }
                    }
                }
            }
            */
        }





        /******************************************************/














        private void OnClick(TickDisplayer tick)
        {
            //Forward event to Board controller
            //tick.Show();
            //OnCardClicked?.Invoke(this, tick);
        }
    }
}