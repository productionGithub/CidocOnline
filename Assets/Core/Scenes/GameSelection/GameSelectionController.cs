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
        public event Action OnBackEvent;

        List<Scenario> _scenarioList = new List<Scenario>();
        ScenarioCompletionModelDown _completions;

        List<string> _languageCriterias = new List<string>();
        List<string> _domainCriterias = new List<string>();


        //public void Show(List<Scenario> entries, ScenarioCompletionModelDown c)
        public void Show(List<Scenario> entries)
        {
            _scenarioList = entries;
            //_completions = c;

            //_panelController.Show(entries, c.Completions);//Show game panels
            _panelController.Show(entries);//Show game panels
            _panelController.OnPanelControllerPlayChapterEvent += OnPlayChapter;

            _languageEntryController.Show(entries);//update Language zone (list of language Toggles)
            _languageEntryController.OnLanguageUpdateEvent += OnFilterLanguagePanel;
            _languageCriterias = _languageEntryController.SelectedLanguages;

            _domainEntryController.Show();
            _domainEntryController.OnDomainUpdateEvent += OnFilterDomainPanel;
            _domainCriterias = _domainEntryController.SelectedDomains;

            _BackButton.onClick.AddListener(BackClickedEvent);
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

        private void OnDestroy()
        {
            OnBackEvent -= BackClickedEvent;
        }
    }
}