using UnityEngine;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Deck;
using StarterCore.Core.Services.GameState;

using System.Collections.Generic;
using Zenject;
using System;


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
            Debug.Log("[Board controller] Init OK");
            _challengeController.Show();//On lui passe tous les challenges (from Manager)

            int currentChallengeId = _gameStateManager.GameStateModel.CurrentChallengeIndex;

            Debug.Log("Challenge list 0 / EleftInit is : " + challengeList[currentChallengeId].ELeftInit);


            //Get init string for ELeft of this challenge
            List<int> initialDeckContent = _entityDeckService.GetInitialDeck(challengeList[12].ELeftInit);

            _leftEntityDeckController.Show(initialDeckContent);

            //1. Pass left deck init value to challenge
            //Init deckController with init value

            //_entityCardsController.Show();
            //_propertyCardsController.Show();
            //_instanceCardsController.Show();
            //_challengeController.Show();//Manage UI events + Evaluation
        }

        //private void OnFilterDomainPanel(List<string> domains)
        //{
        //    _domainCriterias = domains;
        //    Debug.Log("");
        //    FilterScenarii();
        //}

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