#define TRACE_ON
using UnityEngine;
using UnityEngine.UI;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Localization;
using StarterCore.Core.Services.GameState;
using Zenject;
using TMPro;
using System;
using StarterCore.Core.Services.Network;

namespace StarterCore.Core.Scenes.MainMenu
{

    public class MainMenuController : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private MockNetService _networkService;
        [Inject] private GameStateManager _gameState;
        [Inject] private LocalizationManager _localizationManager;

        [SerializeField] TextMeshProUGUI _welcomeTitle;
        [SerializeField] TextMeshProUGUI _continueText;
        [SerializeField] GameObject _continueZone;//Disabled if no history

        [SerializeField] Button _chooseScenario;
        [SerializeField] Button _continue;
        [SerializeField] Button _statistics;
        [SerializeField] Button _leaderBoard;
        [SerializeField] Button _resetGame;
        [SerializeField] Button _quit;
        [SerializeField] Button _goingFurther;
        [SerializeField] Button _fullCredits;

        [SerializeField] GameObject _blurrPanel;
        [SerializeField] GameObject _confirmPanel;
        [SerializeField] GameObject _resetConfirmedPanel;

        [SerializeField] Button _resetYes;
        [SerializeField] Button _resetNo;
        [SerializeField] Button _confirmResetButton;

        [SerializeField] GameObject _waitingIcon;

        public event Action OnChooseScenarioEvent;
        public event Action OnContinueChapterEvent;
        public event Action OnStatisticsEvent;
        public event Action OnLeaderBoardEvent;
        public event Action OnQuitEvent;
        public event Action OnResetGameEvent;
        public event Action OnGoingFurtherEvent;
        public event Action OnFullCreditsEvent;

        public void Init()
        {
            _chooseScenario.onClick.AddListener(OnChooseScenarioButtonClicked);
            _continue.onClick.AddListener(OnContinue);
            _statistics.onClick.AddListener(OnStatistics);
            _leaderBoard.onClick.AddListener(OnLeaderBoard);
            _quit.onClick.AddListener(OnQuit);
            _resetGame.onClick.AddListener(OnResetGame);
            _goingFurther.onClick.AddListener(OnGoingFurther);
            _fullCredits.onClick.AddListener(OnFullCredits);

            _resetYes.onClick.AddListener(OnResetGameConfirmed);
            _resetNo.onClick.AddListener(OnResetGameNotConfirmed);
            _confirmResetButton.onClick.AddListener(OnResetConfirmButton);
        }

        public void ShowWaitingIcon()
        {
            _waitingIcon.SetActive(true);
        }

        public void HideWaitingIcon()
        {
            _waitingIcon.SetActive(false);
        }

        public async void Show()
        {
            //Get player history
            ShowWaitingIcon();
            HistoryModelDown history = await _networkService.GetHistory(_gameStateManager.GameStateModel.UserId);
            HideWaitingIcon();

            //Set 'Continue' choice to waiting state
            _continueZone.SetActive(true);
            _continueText.text = _localizationManager.GetTranslation("mainmenu-scene-continuewaiting-text");
            _continue.interactable = false;// (false);

            if (!history.ScenarioName.Equals(string.Empty))
            {
                //Stats button valid
                _statistics.interactable = true;
                _resetGame.interactable = true;
                //Update game state model
                _gameStateManager.GameStateModel.CurrentScenario = history.ScenarioName;
                _gameStateManager.GameStateModel.CurrentChapter = history.ChapterName;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = Int32.Parse(history.ChallengeId);
                _gameStateManager.GameStateModel.CurrentScore = Int32.Parse(history.Score);
            }
            else
            {
                //Stats button not valid
                _statistics.interactable = false;
                _resetGame.interactable = false;
                _gameStateManager.GameStateModel.CurrentScenario = string.Empty;
                _gameStateManager.GameStateModel.CurrentChapter = string.Empty;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = 0;
                _gameStateManager.GameStateModel.CurrentScore = 0;
            }

            TranslateUI();
        }

        private void TranslateUI()
        {
            _welcomeTitle.text = _localizationManager.GetTranslation("mainmenu-scene-welcometitle-text") + " " + _gameState.Username + " !";
            //Trace.Log("WE BUNDLE ID: " + bundle.HistoryId);

            if(_gameState.GameStateModel.CurrentScenario == string.Empty)
            {
                _continue.interactable = false;// (false);
                _continueText.text = _localizationManager.GetTranslation("mainmenu-scene-nocontinue-text");
            }
            else
            {
                _continue.interactable = true;// (false);
                _continueText.text = _localizationManager.GetTranslation("mainmenu-scene-continue-text") + " " +
                _gameState.GameStateModel.CurrentScenario + "/" +
                _gameState.GameStateModel.CurrentChapter + "/" +
                _gameState.GameStateModel.CurrentChallengeIndex.ToString();
            }
        }

        private void OnContinue()
        {
            OnContinueChapterEvent?.Invoke();
        }

        private void OnChooseScenarioButtonClicked()
        {
            OnChooseScenarioEvent?.Invoke();
        }


        private void OnStatistics()
        {
            OnStatisticsEvent?.Invoke();
        }

        private void OnLeaderBoard()
        {
            OnLeaderBoardEvent?.Invoke();
        }

        private void OnQuit()
        {
            OnQuitEvent?.Invoke();
        }

        private void OnResetGame()
        {
            _blurrPanel.SetActive(true);
            _confirmPanel.SetActive(true);
        }

        private void OnResetGameConfirmed()//Yes on warinig panel
        {
            OnResetGameEvent?.Invoke();
            _confirmPanel.SetActive(false);
            _resetConfirmedPanel.SetActive(true);
        }

        private void OnResetGameNotConfirmed()//NO on warning panel
        {
            _blurrPanel.SetActive(false);
            _confirmPanel.SetActive(false);

        }

        private void OnResetConfirmButton()//OK on Reset successfully done panel
        {
            _resetConfirmedPanel.SetActive(false);
            _blurrPanel.SetActive(false);
            Show();
        }

        private void OnGoingFurther()
        {
            OnGoingFurtherEvent?.Invoke();
        }

        private void OnFullCredits()
        {
            OnFullCreditsEvent?.Invoke();
        }

        public void OnDestroy()
        {
            _chooseScenario.onClick.RemoveListener(OnChooseScenarioButtonClicked);
            _continue.onClick.RemoveListener(OnContinue);
            _statistics.onClick.RemoveListener(OnStatistics);
            _leaderBoard.onClick.RemoveListener(OnLeaderBoard);
            _quit.onClick.RemoveListener(OnQuit);
            _goingFurther.onClick.RemoveListener(OnGoingFurther);
            _fullCredits.onClick.RemoveListener(OnFullCredits);
            _resetYes.onClick.RemoveListener(OnResetGameConfirmed);
            _resetNo.onClick.RemoveListener(OnResetGameNotConfirmed);
            _confirmResetButton.onClick.RemoveListener(OnResetConfirmButton);
        }
    }
}