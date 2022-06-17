using System;
using System.Collections.Generic;
using UnityEngine;


namespace StarterCore.Core.Scenes.Board
{
    public class CardDisplayer : MonoBehaviour
    {
        [SerializeField] private List<TickDisplayer> _tickerList;

        public event Action<CardDisplayer, TickDisplayer> OnCardClicked;


        private void Start()
        {
            Show();
        }

        public void Show()
        {
            foreach (TickDisplayer t in _tickerList)
            {
                t.OnTickerClickEvent += () => OnClick(t);
            }
        }

        private void OnClick(TickDisplayer tick)
        {
            tick.Show();
            OnCardClicked?.Invoke(this, tick);
        }
    }
}
