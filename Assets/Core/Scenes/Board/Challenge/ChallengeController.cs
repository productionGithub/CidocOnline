#define TRACE_ON
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Services.GameState;
using System;

namespace StarterCore.Core.Scenes.Board
{
    public class ChallengeController : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;
        [Inject] NavigationService _navigationService;
        [Inject] ChallengeEvaluator _evaluator;

        public event Action OnCorrectAnswer_ChallengeCtrl;

        [SerializeField]
        private ChallengeDisplayer _displayer;

        private List<ChallengeData> _challengeList;

        public void Init(List<ChallengeData> challengeList)
        {
            _challengeList = challengeList;
            _evaluator.Init();
        }

        public void Show()
        {
            Trace.Log("[ChallengeController] Init OK");
            _displayer.Show(_challengeList);//Statement and other challenge related data
        }

        public bool EvaluateBoard(ChallengeData expectedAnswers, ChallengeData playerAnswers)
        {
            bool isCorrect = _evaluator.CheckAnswers(expectedAnswers, playerAnswers);

            _displayer.DisplayResult(isCorrect, expectedAnswers);
            return isCorrect;
        }
    }
}