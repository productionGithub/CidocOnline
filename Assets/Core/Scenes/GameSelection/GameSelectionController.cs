using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using System.Linq;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionController : MonoBehaviour
    {
        [Header("Static")]
        [SerializeField] private Button _BackButton;
        [SerializeField] private PanelController _panelController;
        [SerializeField] private LanguageEntryController _languageEntryController;
        [SerializeField] private DomainEntryController _domainEntryController;

        public event Action<string, string> OnGameSelectionControllerPlayChapterEvent;
        public event Action<string, string> OnResetProgressionEvent_GameSelectionCtrl;
        public event Action OnBackEvent;

        List<Scenario> _scenarioList = new List<Scenario>();
        ScenarioCompletionModelDown _completions;

        List<string> _languageCriterias = new List<string>();
        List<string> _domainCriterias = new List<string>();

        public void Init()
        {
            _panelController.Init();
            _domainEntryController.OnDomainUpdateEvent += OnFilterDomainPanel;
            _panelController.OnPanelControllerPlayChapterEvent += OnPlayChapter;
            _languageEntryController.OnLanguageUpdateEvent += OnFilterLanguagePanel;
            _panelController.OnResetProgressionEvent_PanelCtrl += OnResetProgression;
            _BackButton.onClick.AddListener(BackClickedEvent);
        }

        //public void Show(List<Scenario> entries, ScenarioCompletionModelDown c)
        public void Show(List<Scenario> entries)// Pass progression?
        {
            _scenarioList = entries;
            _panelController.Show(entries);//Show game panels
            _languageEntryController.Show(entries);//update Language zone (list of language Toggles)
            _languageCriterias = _languageEntryController.SelectedLanguages;
            _domainEntryController.Show();
            _domainCriterias = _domainEntryController.SelectedDomains;
        }

        private void OnFilterDomainPanel(List<string> domains)
        {
            _domainCriterias = domains;
            FilterScenarii();
        }

        public void OnFilterLanguagePanel(List<string> languages)
        {
            _languageCriterias = languages;
            FilterScenarii();
        }

        private void FilterScenarii()
        {
            List<Scenario> filteredScenarioList = new List<Scenario>();

            filteredScenarioList.AddRange(
                _scenarioList.Where(
                    p => p.DomainCodes.Intersect(_domainCriterias).Count() > 0 &&
                    _languageCriterias.Contains(p.LanguageTag))
            );

            //_panelController.Show(filteredScenarioList, _completions.Completions);
            _panelController.Show(filteredScenarioList);
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