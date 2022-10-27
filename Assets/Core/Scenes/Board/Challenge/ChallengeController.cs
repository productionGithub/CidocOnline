using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Challenge;

namespace StarterCore.Core.Scenes.Board
{
    public class ChallengeController : MonoBehaviour
    {
        [Inject] NavigationService _navigationService;

        [SerializeField]
        private ChallengeDisplayer _displayer;
        [SerializeField]
        private ChallengeEvaluator _evaluator;

        public void Show(List<ChallengeData> challengeList)
        {
            Debug.Log("[ChallengeController] Init OK");
            _displayer.Show(challengeList);//Statement
        }

    }
}