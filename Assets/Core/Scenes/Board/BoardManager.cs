using System;
using System.Collections;
using System.Collections.Generic;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;
using StarterCore.Core.Scenes.Board.Controller;
using StarterCore.Core.Scenes.GameSelection;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardManager : IInitializable
    {

        /// <summary>
        /// This manager :
        /// * LISTEN AND PROCESSES CARDSCONTROLLER AND CHALLENGECONTROLLER EVENTS
        /// * FETCH DATA FROM SERVER FOR THE CURRENT CHAPTER PASSED AS BUNDLE
        /// * UPDATE GAMESTATE MODEL
        /// 
        /// [INJECT] CARDS_CONTROLLER
        /// [INJECT] CHALLENGE_CONTROLLER
        /// </summary>


        [Inject] GameStateManager _gameStateManager;
        [Inject] MockNetService _netService;
        [Inject] private BoardController _boardController;

        public void Initialize()
        {

            Debug.Log("[Board manager] Board manager initialized");

            //Debug.Log("[Board manager] Board controller ID is : " + _boardController.GetInstanceID());
            _boardController.Show();
            //_boardController.caca();
            //Init Bundle ?
            //CARDS_CONTROLLER INIT ?
            //CHALLENGE CONTROLLER INIT ?


            //Debug.Log("Gor gameState manager :" + _gameStateManager);
            //Debug.Log("Gor Mock nest service  :" + _netService);

        }
    }
}


/*
 * 
 * 
 *         ///

        //[Inject] CardController cardCtrl;

        public void Initialize()
        {
            //cardCtrl.Show();
            //cardCtrl.OnCardBubbleUp += (CardDisplayer c, TickDisplayer t) => DoSomethingWithIt(c, t);
        }

        private void DoSomethingWithIt(CardDisplayer c, TickDisplayer t)
        {
            Debug.Log(string.Format("[Board Manager] Evaluate Board with {0} and {1}", c.name, t.name));
        }
*/