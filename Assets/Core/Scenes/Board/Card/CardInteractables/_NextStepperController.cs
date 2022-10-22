using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board.Card.CardInteractables
{
    public class NextStepperController : MonoBehaviour
    {
        public event Action OnNextStepperValueChanged;

        [SerializeField]
        Button _stepper;

        public void Show(int maxValue)
        {
            _stepper.onClick.AddListener(OnStepperValueChanged);
        }

        private void OnStepperValueChanged()
        {
            //Debug.Log("[CardSliderController] Value : " + (int) value);
            OnNextStepperValueChanged?.Invoke();
        }
    }
}
