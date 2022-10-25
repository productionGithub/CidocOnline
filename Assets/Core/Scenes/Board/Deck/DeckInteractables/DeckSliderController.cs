using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables
{
    public class DeckSliderController : MonoBehaviour
    {
        public event Action<float> OnSliderValueChangedUI;

        [SerializeField]
        Slider _slider;

        public void Show(int maxValue)
        {
            Debug.Log("[CardSliderController] Init OK");
            SetSliderRange(maxValue);
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            OnSliderValueChangedUI?.Invoke(value);
        }

        public void SetSliderRange(int value)
        {
            _slider.maxValue = value;
            _slider.maxValue = value;
            _slider.value = 0;
        }

        public void SetSliderValue(float value)
        {
            _slider.value = value;
        }
    }
}
