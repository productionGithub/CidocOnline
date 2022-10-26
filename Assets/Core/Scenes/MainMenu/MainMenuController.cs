using UnityEngine;
using UnityEngine.UI;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Localization;
using StarterCore.Core.Services.GameState;
using Zenject;
using TMPro;
using System;

namespace StarterCore.Core.Scenes.MainMenu
{

    public class MainMenuController : MonoBehaviour
    {
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;
        [Inject] private LocalizationManager _localizationManager;

        [SerializeField] TMP_Text _welcomeTitle;
        [SerializeField] TMP_Text _continueText;
        [SerializeField] GameObject _continueZone;//Disabled if no history

        [SerializeField] Button _chooseScenario;

        public event Action OnChooseScenarioEvent;

        HistoryModelDown bundle;

        public void Show()
        {
            //Get data passed from SignIn scene
            bundle = new HistoryModelDown("", "", "", "", "");
            _navService.GetMainBundle(out bundle);

            TranslateUI();

            _chooseScenario.onClick.AddListener(OnChooseScenarioButtonClicked);
        }

        private void TranslateUI()
        {
            _welcomeTitle.text = _localizationManager.GetTranslation("mainmenu-scene-welcometitle-text") + " " + _gameState.Username + " !";
            Debug.Log("WE BUNDLE ID: " + bundle.HistoryId);
            if (bundle.HistoryId.Equals(string.Empty))
            {
                Debug.Log("Disable continue zone !");
                _continueZone.SetActive(false);

            }
            else
            {
                _continueZone.SetActive(true);
                _continueText.text = _localizationManager.GetTranslation("mainmenu-scene-continue-text") + " " +
                bundle.ScenarioName + " / " +
                bundle.ChapterName + " / " +
                bundle.ChallengeId;
            }
        }

        private void OnChooseScenarioButtonClicked()
        {
            OnChooseScenarioEvent?.Invoke();
        }
    }
}