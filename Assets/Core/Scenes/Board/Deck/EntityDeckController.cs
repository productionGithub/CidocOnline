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
        public List<EntityCard> _currentDeckContent;

        [SerializeField] EntityCardController _entityCardsController;
        [SerializeField] CardSliderController _sliderController;
        //[SerializeField] TickController _tickController;

        public event Action<EntityDeckController, float> OnSliderValueChangeDeckController;
        public event Action<float> OnTickFilterDeckController;

        public void Show(List<EntityCard> initialDeck)
        {
            _currentDeckContent = new List<EntityCard>(initialDeck);
            _entityCardsController.Show(_currentDeckContent[0]);

            _sliderController.Show(_currentDeckContent.Count);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;
        }

        private void OnSliderValueChanged(float value)
        {
            //UI deck logic is managed at this controller level
            Debug.Log("[EntityCardController] Slider value is : " + (int)value);
            if (value < _currentDeckContent.Count)//Will be currentDeckContent
            {
                _entityCardsController.Refresh(_currentDeckContent[(int)value]);
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