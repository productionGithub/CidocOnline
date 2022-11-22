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
        [SerializeField] Button _quit;

        public event Action OnChooseScenarioEvent;
        public event Action OnContinueChapterEvent;
        public event Action OnStatisticsEvent;
        public event Action OnQuitEvent;

        public void Init()
        {
            _chooseScenario.onClick.AddListener(OnChooseScenarioButtonClicked);
            _continue.onClick.AddListener(OnContinue);
            _statistics.onClick.AddListener(OnStatistics);
            _quit.onClick.AddListener(OnQuit);
        }

        public async void Show()
        {
            //Set 'Continue' choice to waiting state
            _continueZone.SetActive(true);
            _continueText.text = _localizationManager.GetTranslation("mainmenu-scene-continuewaiting-text");
            _continue.interactable = false;// (false);

            //Get player history
            HistoryModelDown history = await _networkService.GetHistory(_gameStateManager.GameStateModel.UserId);

            if (!history.ScenarioName.Equals(string.Empty))
            {
                //Update game state model
                _gameStateManager.GameStateModel.CurrentScenario = history.ScenarioName;
                _gameStateManager.GameStateModel.CurrentChapter = history.ChapterName;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = Int32.Parse(history.ChallengeId);
                _gameStateManager.GameStateModel.CurrentScore = Int32.Parse(history.Score);

                //Debug.Log("");
            }
            else
            {
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
            Debug.Log("[MainMenuController] Scenario name is" + _gameState.GameStateModel.CurrentScenario);
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

        private void OnQuit()
        {
            OnQuitEvent?.Invoke();
        }

        public void OnDestroy()
        {
            _chooseScenario.onClick.RemoveListener(OnChooseScenarioButtonClicked);
            _continue.onClick.RemoveListener(OnContinue);
            _quit.onClick.RemoveListener(OnStatistics);
            _quit.onClick.RemoveListener(OnQuit);
        }
    }
}