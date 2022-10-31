using System;
using TMPro;
using UnityEngine;


namespace StarterCore.Core.Scenes.Board.Deck
{
    public class DeckCounterDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentSize;
        [SerializeField] private TextMeshProUGUI _initialSize;

        public void Show(int curSize, int initSize)
        {
            _currentSize.GetComponent<TextMeshProUGUI>().text = curSize.ToString();
            _initialSize.GetComponent<TextMeshProUGUI>().text = initSize.ToString();
        }
    }
}