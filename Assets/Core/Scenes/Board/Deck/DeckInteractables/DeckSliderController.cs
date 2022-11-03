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

        public void Init()
        {
            Debug.Log("[CardSliderController] Init OK");
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void Show(int maxValue)
        {
            SetSliderActive(true);//In case last state was 'No card'
            SetSliderRange(maxValue);
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

        public void SetSliderActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
