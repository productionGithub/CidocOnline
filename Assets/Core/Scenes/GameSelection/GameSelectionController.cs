using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionController : MonoBehaviour
    {
        [Header("Static")]
        [SerializeField] private Button _BackButton;
        [SerializeField] private PanelController _panelController;
        [SerializeField] private LanguageEntryController _languageEntryController;
        [SerializeField] private DomainEntryController _domainEntryController;
        [SerializeField] private GameObject _waitingIcon;

        public event Action<string, string> OnGameSelectionControllerPlayChapterEvent;
        public event Action<string, string> OnResetProgressionEvent_GameSelectionCtrl;
        public event Action OnBackEvent;

        List<Scenario> _scenarioList = new List<Scenario>();

        private List<string> _languageCriterias = new List<string>();
        private List<string> _domainCriterias = new List<string>();

        private List<ChapterProgressionModelDown> _userProgression;

        private List<ChallengeData> _challenges;

        public void Init()
        {
            _panelController.Init();
            _domainEntryController.OnDomainUpdateEvent += OnFilterDomainPanel;
            _panelController.OnPanelControllerPlayChapterEvent += OnPlayChapter;
            _languageEntryController.OnLanguageUpdateEvent += OnFilterLanguagePanel;
            _panelController.OnResetProgressionEvent_PanelCtrl += OnResetProgression;
            _BackButton.onClick.AddListener(BackClickedEvent);
        }

        public void ShowWaitingIcon()
        {
            _waitingIcon.SetActive(true);
            _panelController.gameObject.SetActive(false);
        }
        public void HideWaitingIcon()
        {
            _waitingIcon.SetActive(false);
            _panelController.gameObject.SetActive(true);
        }


        private void OnFilterDomainPanel(List<string> domains)
        {
            OnFilterDomainPanelAsync(domains);
        }

        private void OnFilterLanguagePanel(List<string> langages)
        {
            OnFilterLanguagePanelAsync(langages);
        }

        public void Show(List<Scenario> catalog, List<ChapterProgressionModelDown> userProgression)
        {
            _userProgression = userProgression;

            Debug.Log("");

            _scenarioList = catalog;

            _panelController.Show(catalog, userProgression);//Show game panels
            _languageEntryController.Show(catalog);//update Language zone (list of language Toggles)
            _languageCriterias = _languageEntryController.SelectedLanguages;
            _domainEntryController.Show();
            _domainCriterias = _domainEntryController.SelectedDomains;
        }

        private void OnFilterDomainPanelAsync(List<string> domains)
        {
            _domainCriterias = domains;
            FilterScenarii(_userProgression);
        }

        public void OnFilterLanguagePanelAsync(List<string> languages)
        {
            _languageCriterias = languages;
            FilterScenarii(_userProgression);
        }

        private void FilterScenarii(List<ChapterProgressionModelDown> userProgression)
        {
            List<Scenario> filteredScenarioList = new List<Scenario>();

            filteredScenarioList.AddRange(
                _scenarioList.Where(
                    p => p.DomainCodes.Intersect(_domainCriterias).Count() > 0 &&
                    _languageCriterias.Contains(p.LanguageTag))
            );

            _panelController.Show(filteredScenarioList, userProgression);
        }

        private void OnPlayChapter(string scenarioTitle, string chapterTitle)
        {
            OnGameSelectionControllerPlayChapterEvent?.Invoke(scenarioTitle, chapterTitle);
        }

        private void BackClickedEvent()
        {
            OnBackEvent?.Invoke();
        }

        private void OnResetProgression(string chapterName, string scenarioName)
        {
            Debug.Log("RESET ! -> GameSelectionController");
            OnResetProgressionEvent_GameSelectionCtrl?.Invoke(chapterName, scenarioName);
        }

        private void OnDestroy()
        {
            _domainEntryController.OnDomainUpdateEvent -= OnFilterDomainPanel;
            _panelController.OnPanelControllerPlayChapterEvent -= OnPlayChapter;
            _languageEntryController.OnLanguageUpdateEvent -= OnFilterLanguagePanel;
            _panelController.OnResetProgressionEvent_PanelCtrl -= OnResetProgression;
            _BackButton.onClick.RemoveListener(BackClickedEvent);
            OnBackEvent -= BackClickedEvent;
        }
    }
}