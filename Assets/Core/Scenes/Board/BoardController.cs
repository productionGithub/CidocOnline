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
        [Inject] PropertyDeckService _propertyDeckService;

        [SerializeField] ChallengeController _challengeController;
        [SerializeField] EntityDeckController _leftEntityDeckController;
        [SerializeField] PropertyDeckController _leftPropertyDeckController;

        public void Show(List<ChallengeData> challengeList)
        {
            //ChallengeCtrl will not ne a mono. Data management only.
            //_challengeController.Show();//On lui passe tous les challenges (from Manager)

            int currentChallengeId = _gameStateManager.GameStateModel.CurrentChallengeIndex;

            //Initialization of Left Entity Deck
            List<EntityCard> initialLeftEntityDeckContent = _entityDeckService.GetInitialDeck(challengeList[currentChallengeId].ELeftInit);
            _leftEntityDeckController.Show(initialLeftEntityDeckContent);

            //Initialization of Left Property Deck
            List<PropertyCard> initialLeftPropertyDeckContent = _propertyDeckService.GetInitialDeck(challengeList[currentChallengeId].PLeftInit);
            _leftPropertyDeckController.Show(initialLeftPropertyDeckContent);
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