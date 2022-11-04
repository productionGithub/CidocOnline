#define TRACE_ON
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.Board
{
    public class ChallengeController : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;
        [Inject] NavigationService _navigationService;
        [Inject] ChallengeEvaluator _evaluator;

        private readonly string _success = "Bravo !";

        [SerializeField]
        private ChallengeDisplayer _displayer;

        //Player's answers are stored in a copy of the challenges[currentChallengeId] 'ChallengeData' object
        //'__Anwsers' properties of this copy are update with players answers
        // This copy is then compared with expected results in the original ChallengeData object when the Validate button is pressed.
        public ChallengeData playerChallengeAnswers;

        private List<ChallengeData> _challengeList;

        public void Init(List<ChallengeData> challengeList)
        {
            _challengeList = challengeList;
            _evaluator.Init();
        }

        public void Show()
        {
            Debug.Log("[ChallengeController] Init OK");
            _displayer.Show(_challengeList);//Statement and other challenge related data
        }

        public bool EvaluateBoard(ChallengeData expectedAnswers, ChallengeData playerAnswers)
        {
            bool isCorrect = _evaluator.CheckAnswers(expectedAnswers, playerAnswers);
            if(isCorrect)
            {

                _gameStateManager.GameStateModel.CurrentScore += expectedAnswers.Score;
            }
            _displayer.DisplayResult(isCorrect, expectedAnswers);
            return isCorrect;

        }
    }
}