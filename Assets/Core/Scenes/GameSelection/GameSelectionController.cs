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

        List<string> _languageCriterias = new List<string>();
        List<string> _domainCriterias = new List<string>();


        public void Show(List<Scenario> entries)
        {
            _scenarioList = entries;

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
            Debug.Log("");
            FilterScenarii();
        }

        public void OnFilterLanguagePanel(List<string> languages)
        {
            _languageCriterias = languages;
            FilterScenarii();
        }


        //TODO Refactor to have a list scenario object?
        private void FilterScenarii()
        {
            List<Scenario> filteredScenarioList = new List<Scenario>();

            filteredScenarioList.AddRange(
                _scenarioList.Where(
                    p => p.DomainCodes.Intersect(_domainCriterias).Count() > 0 &&
                    _languageCriterias.Contains(p.LanguageTag))
            );

            foreach (var Scenario in filteredScenarioList)
            {
                Debug.Log("Filtered scenarioList : " + Scenario.ScenarioTitle);
            }

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