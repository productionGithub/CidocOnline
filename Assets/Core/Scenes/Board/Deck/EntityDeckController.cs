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

        //[SerializeField] TickController _tickController;

        public event Action<float> OnTickFilterDeckController;

        public void Show(List<EntityCard> initialDeck)
        {
            _addedCard = new List<EntityCard>();
            _currentDeckContent = new List<EntityCard>(initialDeck);

            _entityCardDisplayer.Show(initialDeck[0]);

            _hierarchyDisplayer.Show(initialDeck[0]);
            _hierarchyDisplayer.HierarchyEntDisp_HierarchyClickEvent += OnHierarchyEntityClick;

            _sliderController.Show(_currentDeckContent.Count - 1);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

            _previousStepper.onClick.AddListener(OnPreviousCardClicked);
            _nextStepper.onClick.AddListener(OnNextCardClicked);

            _deckCounterDisplayer.Show(
                initialDeck.Count.ToString(), _currentDeckContent.Count.ToString());
        }

        private void OnHierarchyEntityClick(string cardId)
        {
            EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == cardId);
            DisplayCard(card);
        }

        private void DisplayCard(EntityCard card)
        {
            //If newCard exists, just Refresh the card displayer with it
            if (_currentDeckContent.Exists(x => x.id.Equals(card.id)))
            {
                int index = _currentDeckContent.FindIndex(c => c.id == card.id);
                _entityCardDisplayer.Refresh(_currentDeckContent[index]);
                _sliderController.SetSliderValue(index);
            }

            else //If newCard does not exist add it to the initial deck
            {
                _currentDeckContent.Add(card);//add new card in deck, but...
                _addedCard.Add(card);//it is not part of initial deck so add it to this list
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);//Deck has 1 more card, update slider range

                _entityCardDisplayer.Refresh(_currentDeckContent[_currentDeckContent.Count - 1]);

                _sliderController.SetSliderValue(_currentDeckContent.Count - 1);//Set slider to 'the end' of deck (=== size of deck)

                _deckCounterDisplayer.Show(
                    _currentDeckContent.Count.ToString(), _currentDeckContent.Count.ToString());
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



        private void OnClick(TickDisplayer tick)
        {
            //Forward event to Board controller
            //tick.Show();
            //OnCardClicked?.Invoke(this, tick);
        }
    }
}