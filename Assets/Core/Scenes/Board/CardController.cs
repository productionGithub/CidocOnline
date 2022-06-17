using UnityEngine;
using System;

namespace StarterCore.Core.Scenes.Board
{
    public class CardController : MonoBehaviour
    {

        [SerializeField] private CardDisplayer _leftCard;
        [SerializeField] private CardDisplayer _rightCard;

        public Action<CardDisplayer, TickDisplayer> OnCardBubbleUp;

        public void Show()
        {
            _leftCard.OnCardClicked += (CardDisplayer c, TickDisplayer t) => OnCardDisplayerClicked(c, t);
            _rightCard.OnCardClicked += (CardDisplayer c, TickDisplayer t) => OnCardDisplayerClicked(c, t);
        }



        //Little workaround to debug.log the name of the the ticker that has been clicked
        private void OnCardDisplayerClicked(CardDisplayer card, TickDisplayer tick)
        {
            Debug.Log("[CardController] Card clicked : " + card.name + "--->" + tick.name);
            OnCardBubbleUp?.Invoke(card, tick);
        }
    }
}
