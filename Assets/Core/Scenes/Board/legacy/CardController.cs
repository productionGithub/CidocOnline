using UnityEngine;
using System;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Scenes.GameSelection;
using TMPro;

using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.Board
{
    public class CardController : MonoBehaviour
    {
        /*
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        [SerializeField] private TextMeshProUGUI _username;
        [SerializeField] private CardDisplayer _leftCard;
        [SerializeField] private CardDisplayer _rightCard;

        public Action<CardDisplayer, TickDisplayer> OnCardBubbleUp;

        public void Show()
        {
            ChapterInfoBundle bundle = new ChapterInfoBundle("", "");
            _navService.GetMainBundle(out bundle);
            _username.text = _gameState.Username;

            Debug.Log(string.Format("Bundle values {0} / {1}", bundle._chapterTitle, bundle._scenarioTitle));
            _leftCard.OnCardClicked += (CardDisplayer c, TickDisplayer t) => OnCardDisplayerClicked(c, t);
            _rightCard.OnCardClicked += (CardDisplayer c, TickDisplayer t) => OnCardDisplayerClicked(c, t);
        }

        //Little workaround to debug.log the name of the the ticker that has been clicked
        private void OnCardDisplayerClicked(CardDisplayer card, TickDisplayer tick)
        {
            Debug.Log("[CardController] Card clicked : " + card.name + "--->" + tick.name);
            OnCardBubbleUp?.Invoke(card, tick);
        }
        */
    }
}
