#define TRACE_ON
using System.Collections.Generic;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;
using TMPro;
using UnityEngine;
using Zenject;
using System.Linq;
using StarterCore.Core.Services.Localization;

namespace StarterCore.Core.Scenes.Board.Challenge
{
    public class ChallengeDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        [Inject] GameStateManager _gameStateManager;
        [Inject] LocalizationManager _localizationManager;

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
        GameObject _winBanner;
        [SerializeField]
        GameObject _explanationTitle;
        [SerializeField]
        GameObject _explanationDescription;
        [SerializeField]
        GameObject _boardMask;
        [SerializeField]
        GameObject _winIcon;
        [SerializeField]
        GameObject _looseIcon;

        //Score zone
        [SerializeField]
        private TextMeshProUGUI _currentScore;
        [SerializeField]
        private TextMeshProUGUI _maxScore;

        //
        public int currentChallengeId;
        public Chapter currentChapter;

        public void Show(List<ChallengeData> challengeList)
        {
            ChallengeData currentChallenge = challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex];
            Trace.Log("[Challenge Displayer] Current challenge is : " + _gameStateManager.GameStateModel.CurrentChallengeIndex);
            Trace.Log("[Challenge Displayer] ELeftAnswer expected answers is : " + currentChallenge.ELeftAnswer);

            //Hide message and explanation texts
            _explanationTitle.SetActive(false);
            _explanationDescription.SetActive(false);

            _scenarioTitle.text = _gameStateManager.GameStateModel.CurrentScenario;
            _chapterTitle.text = _gameStateManager.GameStateModel.CurrentChapter;

            _challengeTitle.text = currentChallenge.Title;

            //Challenge counter
            _challengeCurrentIndex.text = _gameStateManager.GameStateModel.CurrentChallengeIndex.ToString();
            _nbTotalChallenges.text = (challengeList.Count -1).ToString();//Challenge 0 is an info challenge

            _statementText.text = currentChallenge.Statement;

            _currentScore.text = _gameStateManager.GameStateModel.CurrentScore.ToString();
            int maximumScore = challengeList.Sum(c => c.Score);
            _maxScore.text = maximumScore.ToString();

            _winBanner.SetActive(false);
            _explanationTitle.SetActive(false);
            _explanationDescription.SetActive(false);


            _winIcon.SetActive(false);
            _looseIcon.SetActive(false);

            _boardMask.SetActive(false);
        }

        public void DisplayResult(bool answerCorrect, ChallengeData expectedAnswers)
        {
            _boardMask.SetActive(true);
            if (answerCorrect)
            {
                _currentScore.text = _gameStateManager.GameStateModel.CurrentScore.ToString();
                _explanationTitle.SetActive(false);
                _explanationDescription.SetActive(false);
                _winBanner.SetActive(true);
                _winIcon.SetActive(true);
            }
            else
            {
                _winBanner.SetActive(false);
                //Display explanation message
                _explanationTitle.GetComponentInChildren<TextMeshProUGUI>().text = _localizationManager.GetTranslation("boardscene-scene-explanationtitle-text");
                _explanationTitle.SetActive(true);
                _explanationDescription.GetComponentInChildren<TextMeshProUGUI>().text = expectedAnswers.Explanation;
                _explanationDescription.SetActive(true);
                _looseIcon.SetActive(true);
            }
        }
    }
}
