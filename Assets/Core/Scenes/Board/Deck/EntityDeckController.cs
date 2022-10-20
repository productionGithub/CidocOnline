using StarterCore.Core.Scenes.Board.Card.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class EntityDeckController : MonoBehaviour
    {
        /// <summary>
        /// Manage current deck state
        /// Controls deck displayer
        /// Controls deck interactables (Slider + Ticks)
        /// </summary>
        /// 
        [Inject] EntityDeckService _entityDeckService;
        public Dictionary<string, EntityCard> _currentDeckContent = new Dictionary<string, EntityCard>();

        [SerializeField] EntityCardController _entityCardsController;
        [SerializeField] CardSliderController _sliderController;
        //[SerializeField] TickController _tickController;

        public event Action<EntityDeckController, float> OnSliderValueChangeDeckController;
        public event Action<float> OnTickFilterDeckController;

        public void Show(List<int> listOfEntityCardIndex)
        {
            InitCurrentDeck(listOfEntityCardIndex);
            var first = _currentDeckContent.OrderBy(kvp => kvp.Key).First();
            string firstKey = first.Key;
            _entityCardsController.Show(_currentDeckContent[firstKey]);

            _sliderController.Show(_entityDeckService.EntityCards.Count);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;
        }

        private void InitCurrentDeck(List<int> indexOfCards)
        {
            foreach (int index in indexOfCards)
            {
                _currentDeckContent.Add(index.ToString(), _entityDeckService.EntityCards[index]);
            }
        }
        private void OnSliderValueChanged(float value)
        {
            //UI deck logic is managed at this controller level
            Debug.Log("[EntityCardController] Slider value is : " + (int)value);
            if (value < _entityDeckService.EntityCards.Count)//Will be currentDeckContent
            {
                _entityCardsController.Refresh(_entityDeckService.EntityCards[(int)value]);
            }

            OnSliderValueChangeDeckController?.Invoke(this, value);
        }

        private void OnClick(TickDisplayer tick)
        {
            //Forward event to Board controller
            //tick.Show();
            //OnCardClicked?.Invoke(this, tick);
        }
    }
}