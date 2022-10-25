using UnityEngine;
using System;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Card.Cards;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables
{
    public class TBDAddCard : MonoBehaviour
    {
        public event Action OnAddCard;

        [SerializeField]
        Button _button;

        public void Show()
        {
            _button.onClick.AddListener(OnAddCardToDeck);
        }

        private void OnAddCardToDeck()
        {
            Debug.Log("[TBDAddCardController] CLICKED ");
            OnAddCard?.Invoke();
        }
    }
}
