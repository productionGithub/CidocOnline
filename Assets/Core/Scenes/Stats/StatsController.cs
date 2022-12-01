using System;
using UnityEngine;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Stats
{
    public class StatsController : MonoBehaviour
    {
        [Inject] DiContainer _diContainer;

        [SerializeField] private ScenarioPanelEntry _scenarioPanelTemplate;
        [SerializeField] private Transform _panelContainer;
        [SerializeField] private TextMeshProUGUI _grandTotalProgressionPercentageTxt;
        [SerializeField] private TextMeshProUGUI _grandTotalScoreTxt;
        [SerializeField] private TextMeshProUGUI _grandTotalMaximumPossibleScoreTxt;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] GameObject _waitingIcon;
        [SerializeField] GameObject _statsScrollView;
        [SerializeField] GameObject _grandTotal;

        public event Action OnMainMenuEvent;

        private List<ScenarioPanelEntry> _entriesList;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<ScenarioPanelEntry>();
            }
            else
            {
                foreach (ScenarioPanelEntry e in _entriesList)
                {
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        public void ShowWaitingIcon()
        {
            _waitingIcon.SetActive(true);
            _statsScrollView.SetActive(false);
            _grandTotal.SetActive(false);
        }
        public void HideWaitingIcon()
        {
            _waitingIcon.SetActive(false);
            _statsScrollView.SetActive(true);
            _grandTotal.SetActive(true);
        }

        public void Show(List<ChapterProgressionModelDown> userProgressions)
        {
            Debug.Log("[StatsController] Show !");
            _scenarioPanelTemplate.gameObject.SetActive(false);

            List<string> scenarii = new List<string>();
            List<string> chapterLines = new List<string>();

            foreach (IEnumerable sname in userProgressions.Select(o => o.ScenarioName).Distinct())
            {
                scenarii.Add(sname.ToString());
            }

            foreach (string s in scenarii)
            {
                ScenarioPanelEntry instance = Instantiate(_scenarioPanelTemplate, _panelContainer);
                _diContainer.InjectGameObject(instance.gameObject);

                _entriesList.Add(instance);

                instance.gameObject.SetActive(true);

                instance.Init();
                instance.Show(s, userProgressions);
            }

            int grandTotalProgression = 0;
            int grandTotalScore = 0;
            int grandTotalMaximumPossibleScore = 0;
            int grandTotalNbChapters = 0;

            foreach (ScenarioPanelEntry scenarioEntry in _entriesList)
            {
                grandTotalProgression += scenarioEntry.totalScenarioProgression;
                grandTotalScore += scenarioEntry.totalScenarioScore;
                grandTotalMaximumPossibleScore += scenarioEntry.totalScenarioMaxPossibleScore;
                grandTotalNbChapters += scenarioEntry.nbChapterInScenario;
            }

            _grandTotalProgressionPercentageTxt.text = (grandTotalProgression * 100) / (grandTotalNbChapters * 100)+"%";
            _grandTotalScoreTxt.text = grandTotalScore.ToString();
            _grandTotalMaximumPossibleScoreTxt.text = grandTotalMaximumPossibleScore.ToString();
        }

        private void OnMainMenuClicked()
        {
            OnMainMenuEvent?.Invoke();
        }

        public void OnDestroy()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }
    }
}