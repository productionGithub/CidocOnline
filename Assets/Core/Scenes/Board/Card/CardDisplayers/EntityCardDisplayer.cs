using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class EntityCardDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        //Card fields
        //[SerializeField]
        //private GameObject icon1 = null;
        //[SerializeField]
        //private GameObject icon2;
        public TextMeshProUGUI Id;
        //[SerializeField]
        //private GameObject label;
        //[SerializeField]
        //private GameObject scrollView;
        //[SerializeField]
        //private GameObject classHierachyContent;
        //[SerializeField]
        //private GameObject colorBarLeftTop;
        //[SerializeField]
        //private GameObject colorBarLeftBottom;
        //[SerializeField]
        //private GameObject colorBarRightTop;
        //[SerializeField]
        //private GameObject colorBarRightBottom;
        //[SerializeField]
        //private GameObject comment;

        //Slider
        [SerializeField] public CardSliderController _sliderController;

        public event Action<float> OnSliderValueChangedDisplayer; // Bubble up event
        //public event Action<EntityCardDisplayer, TickDisplayer> OnCardClicked;


        //private void Start()
        //{
        //    Show();
        //}

        public void Show()
        {
            _sliderController.OnSliderValueChangedUI += OnSliderValueChanged;
            _sliderController.Show(gameObject);
        }

        private void OnSliderValueChanged(float value)
        {
            //Id.text = value.ToString();
            OnSliderValueChangedDisplayer?.Invoke(value);
        }

        private void OnClick(TickDisplayer tick)
        {
            //tick.Show();
            //OnCardClicked?.Invoke(this, tick);
        }
    }
}
