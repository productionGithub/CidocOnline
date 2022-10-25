using System;
using TMPro;
using UnityEngine;


namespace StarterCore.Core.Scenes.Board.Deck
{
    public class DeckCounterDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentSize;
        [SerializeField] private TextMeshProUGUI _maxSize;

        public event Action<string> OnDetailEntryPlayEvent;

        public void Show(string curSize, string maxSize)
        {
            _currentSize.GetComponent<TextMeshProUGUI>().text = curSize;
            _currentSize.GetComponent<TextMeshProUGUI>().text = maxSize;
        }
    }
}