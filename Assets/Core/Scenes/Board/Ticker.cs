
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board
{
    // One element of the list, in the view

    public class Ticker: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int PositionOffset;

        [SerializeField] private PointerEventData.InputButton _tickerImage;

        public event Action OnTickerClickEvent;

        public bool isOn = false;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == _tickerImage)
            {
                OnTickerClickEvent?.Invoke();
            }
        }

        public void Show()
        {
            int offset = (isOn) ? PositionOffset : -PositionOffset;

            Vector3 newPos = new(0f, offset, 0f);
            isOn = !isOn;
            transform.position += newPos;
        }
    }
}