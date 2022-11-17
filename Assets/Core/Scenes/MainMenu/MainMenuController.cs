using UnityEngine;
using UnityEngine.UI;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Navigation;
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
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;
        [Inject] private LocalizationManager _localizationManager;

        [SerializeField] TextMeshProUGUI _welcomeTitle;
        [SerializeField] TextMeshProUGUI _continueText;
        [SerializeField] GameObject _continueZone;//Disabled if no history

        [SerializeField] Button _chooseScenario;
        [SerializeField] Button _continue;

        public event Action OnChooseScenarioEvent;
        public event Action OnContinueChapter;

        HistoryModelDown bundle;

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
                _gameStateManager.GameStateModel.CurrentScore = Int32.Parse(history.Score.ToString());

                Debug.Log("");
            }
            else
            {
                _gameStateManager.GameStateModel.CurrentScenario = string.Empty;
                _gameStateManager.GameStateModel.CurrentChapter = string.Empty;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = 0;
                _gameStateManager.GameStateModel.CurrentScore = 0;
            }

            Trace.Log(string.Format("[MaineMenuController] Scenario name from model is : ", _gameState.GameStateModel.CurrentScenario));

            TranslateUI();

            _chooseScenario.onClick.AddListener(OnChooseScenarioButtonClicked);
            _continue.onClick.AddListener(OnContinue);
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
            OnContinueChapter?.Invoke();
        }

        private void OnChooseScenarioButtonClicked()
        {
            OnChooseScenarioEvent?.Invoke();
        }
    }
}