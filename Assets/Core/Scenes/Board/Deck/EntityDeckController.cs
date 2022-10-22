using StarterCore.Core.Scenes.Board.Card.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Card.CardInteractables;

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
        public List<EntityCard> _AddedCard;

        [SerializeField] EntityCardController _entityCardController;
        [SerializeField] CardSliderController _sliderController;
        [SerializeField] Button _previousStepper;
        [SerializeField] Button _nextStepper;

        [SerializeField] TBDAddCard _adder;
        [SerializeField] Image _marker;

        //[SerializeField] TickController _tickController;

        public event Action<EntityDeckController, float> OnSliderValueChangeDeckController;
        //public event Action OnPreviousStepperValueChangedController;
        //public event Action OnNextStepperValueChangedController;

        public event Action<float> OnTickFilterDeckController;

        public void Show(List<EntityCard> initialDeck)
        {
            _AddedCard = new List<EntityCard>();
               _currentDeckContent = new List<EntityCard>(initialDeck);
            _entityCardController.Show(_currentDeckContent[0]);
            Debug.Log("[EntityDeckController] NB cards in deck : " + _currentDeckContent.Count);
            _sliderController.Show(_currentDeckContent.Count - 1);
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;

            _previousStepper.onClick.AddListener(OnPreviousCardClicked);
            _nextStepper.onClick.AddListener(OnNextCardClicked);

            _adder.Show();
            _adder.OnAddCard += AddCardToDeck;
            _marker.gameObject.SetActive(false);
        }

        private void AddCardToDeck()
        {
            EntityCard newCard = _entityDeckService.EntityCards[UnityEngine.Random.Range(0, _currentDeckContent.Count - 1)];
            if (!_currentDeckContent.Exists(x => x.id.Equals(newCard.id)))
            {
                _currentDeckContent.Add(newCard);
                _AddedCard.Add(newCard);

                //Debug.Log("New size : " + _currentDeckContent.Count);
                _sliderController.SetSliderRange(_currentDeckContent.Count - 1);
                _sliderController.SetSliderValue(0);
                _entityCardController.Refresh(_currentDeckContent[0]);
            }
            //Debug.Log("Added card : " + newCard.id);
        }

        private void OnSliderValueChanged(float value)
        {
            //UI deck logic is managed at this controller level
            //Debug.Log("Current value is : " + _sliderController.GetComponent<Slider>().value);
            if (value < _currentDeckContent.Count - 1 || value > 0)//Will be currentDeckContent
            {
                _entityCardController.Refresh(_currentDeckContent[(int)value]);

                if (_AddedCard.Exists(x => x.id.Equals(_currentDeckContent[(int)value].id)))
                {
                    Debug.Log("Added card !");
                   _entityCardController.GhostBackground();
                   // _marker.gameObject.SetActive(true);
                }
                else
                {
                    _entityCardController.ReinitBackground();
                    //_marker.gameObject.SetActive(false);
                }
                    
            }

            OnSliderValueChangeDeckController?.Invoke(this, value);
        }

        private void OnPreviousCardClicked()
        {
            if (_sliderController.GetComponent<Slider>().value > 0)
            {
                float previousCardIndex = _sliderController.GetComponent<Slider>().value - 1;
                //Debug.Log("[NextCardClicked] nextCardIndex float value is " + _sliderController.GetComponent<Slider>().value);
                //Debug.Log("[NextCardClicked] nextCardIndex int value is " + (int)previousCardIndex);
                _entityCardController.Refresh(_currentDeckContent[(int)previousCardIndex]);
                //Set new slider position
                _sliderController.SetSliderValue(previousCardIndex);
            }
        }

        private void OnNextCardClicked()
        {
            if (_sliderController.GetComponent<Slider>().value < _currentDeckContent.Count - 1)
            {
                //Debug.Log("[NextCardClicked] Slider value is " + _sliderController.GetComponent<Slider>().value);
                float nextCardIndex = _sliderController.GetComponent<Slider>().value + 1;
                //Debug.Log("[NextCardClicked] nextCardIndex float value is " + _sliderController.GetComponent<Slider>().value);
                //Debug.Log("[NextCardClicked] nextCardIndex int value is " + (int) nextCardIndex);
                _entityCardController.Refresh(_currentDeckContent[(int)nextCardIndex]);

                if (_AddedCard.Exists(x => x.id.Equals(_currentDeckContent[(int)nextCardIndex].id)))
                {
                    Debug.Log("Added card !");
                    _entityCardController.GhostBackground();
                    //_marker.gameObject.SetActive(true);
                }
                else
                {
                    _entityCardController.ReinitBackground();
                    //_marker.gameObject.SetActive(false);
                }


                //Set new slider position
                _sliderController.SetSliderValue(nextCardIndex);
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