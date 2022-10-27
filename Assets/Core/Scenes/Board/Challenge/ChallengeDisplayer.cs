using System;
using System.Collections.Generic;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StarterCore.Core.Scenes.Board.Challenge
{
    public class ChallengeDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        [Inject] NavigationService _navigationService;
        [Inject] EntityDeckService _entityDeckService;
        [Inject] GameStateManager _gameStateManager;

        //Top challenge infos
        [SerializeField]
        private TextMeshProUGUI _scenarioTitle;
        [SerializeField]
        private TextMeshProUGUI _chapterTitle;
        [SerializeField]
        private TextMeshProUGUI _challengeTitle;

        //Challenge counter
        [SerializeField]
        private TextMeshProUGUI _challengeCurrentIndex;
        [SerializeField]
        private TextMeshProUGUI _nbTotalChallenges;

        //Statement
        [SerializeField]
        private TextMeshProUGUI _statementText;

        //Top text zones
        [SerializeField]
        GameObject _defaultMessage;
        [SerializeField]
        GameObject _resultMessage;
        [SerializeField]
        GameObject _explanationMessage;

        //Score zone
        [SerializeField]
        private TextMeshProUGUI _currentScore;
        [SerializeField]
        private TextMeshProUGUI _maxScore;



        public int currentChallengeId;
        public Chapter currentChapter;

        //Player's answers are stored in a copy of the challenges[currentChallengeId] 'ChallengeData' object
        //'__Anwsers' properties of this copy are update with players answers
        // This copy is then compared with expected results in the original ChallengeData object when the Validate button is pressed.
        public ChallengeData playerChallengeAnswers;

        //Utility class for parsing ChallengeData fields 
        //ParserTool parser = new ParserTool();

        readonly string defaultTopTextZone = "CIDOC-CRM GAME";

        public void Show(List<ChallengeData> challengeList)
        {
            ChallengeData currentChallenge = challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex];


            //Hide message and explanation texts
            _resultMessage.SetActive(false);
            _explanationMessage.SetActive(false);

            _scenarioTitle.text = _gameStateManager.GameStateModel.CurrentScenario;
            _chapterTitle.text = _gameStateManager.GameStateModel.CurrentChapter;

            _challengeTitle.text = currentChallenge.Title;

            //Challenge counter
            _challengeCurrentIndex.text = _gameStateManager.GameStateModel.CurrentChallengeIndex.ToString();
            _nbTotalChallenges.text = (challengeList.Count -1).ToString();//Challenge 0 is an info challenge

            _statementText.text = currentChallenge.Statement;

            _currentScore.text = "999";
            _maxScore.text = "999";

            _defaultMessage.SetActive(true);
            _defaultMessage.GetComponentInChildren<TextMeshProUGUI>().text = defaultTopTextZone;
        }
    }
}
