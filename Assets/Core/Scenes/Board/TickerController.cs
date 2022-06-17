using System;
using System.Collections.Generic;
using UnityEngine;


namespace StarterCore.Core.Scenes.Board
{
    public class TickerController : MonoBehaviour
    {
        [SerializeField] private List<Ticker> _tickerList;

        public event Action<Ticker> OnTickSet;


        public void Show()
        {
            foreach (Ticker t in _tickerList)
            {
                t.OnTickerClickEvent += () => OnClick(t);
            }
        }

        private void OnClick(Ticker tick)
        {
            tick.Show();
            OnTickSet?.Invoke(tick);
        }


        //Little workaround to display logic of the TickerManager
        public void DisplayMsg(string s)
        {
            Debug.Log(s);
        }
    }
}
