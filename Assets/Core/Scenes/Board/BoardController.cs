using UnityEngine;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Deck;
using StarterCore.Core.Services.GameState;

using System.Collections.Generic;
using Zenject;
using System;
using StarterCore.Core.Scenes.Board.Card.Cards;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardController : MonoBehaviour
    {
        /// <summary>
        /// Pass board UI events to manager (Pause, Blurr panel, Validate button, Arrow animation, ...),
        /// challenge events and decks events.
        /// Has ref to a challenge controller (UI content and Evaluation).
        /// Has ref to Entity, Property and Instance cards.
        /// </summary>

        [Inject] NavigationService _netService;
        [Inject] GameStateManager _gameStateManager;
        [Inject] EntityDeckService _entityDeckService;

        [SerializeField] ChallengeController _challengeController;// SHould be at challenge level?
        [SerializeField] EntityDeckController _leftEntityDeckController;// SHould be at challenge level?


        public void Show(List<ChallengeData> challengeList)
        {
            //ChallengeCtrl will not ne a mono. Data management only.
            //_challengeController.Show();//On lui passe tous les challenges (from Manager)

            int currentChallengeId = _gameStateManager.GameStateModel.CurrentChallengeIndex;

            //Initialization of Left Entity Deck
            List<EntityCard> initialDeckContent = _entityDeckService.GetInitialDeck(challengeList[currentChallengeId].ELeftInit);
            _leftEntityDeckController.Show(initialDeckContent);
        }

        private void OnGamePaused()
        {
        }

        private void OnAnimateArrow()
        {
        }

        private void OntickClicked()
        {
        }

        private void OnDestroy()
        {
        }
    }
}