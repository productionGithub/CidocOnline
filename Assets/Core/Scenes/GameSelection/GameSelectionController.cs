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
        [SerializeField] private TopicEntryController _topicEntryController;
        [SerializeField] private GameObject _waitingIcon;

        public event Action<string, string> OnGameSelectionControllerPlayChapterEvent;
        public event Action<string, string> OnResetProgressionEvent_GameSelectionCtrl;
        public event Action OnBackEvent;

        List<Scenario> _scenarioList = new List<Scenario>();

        private List<string> _languageCriterias = new List<string>();
        private List<string> _topicCriteria = new List<string>();

        private List<ChapterProgressionModelDown> _userProgression;

        //private List<ChallengeData> _challenges;

        public void Init()
        {
            Debug.Log("GAME SELECTION CONTROLLER INIT !");

            _panelController.Init();
            _topicEntryController.OnTopicUpdateEvent += OnFilterTopicPanel;
            _panelController.OnPanelControllerPlayChapterEvent += OnPlayChapter;
            _languageEntryController.OnLanguageUpdateEvent += OnFilterLanguagePanel;
            _panelController.OnResetProgressionEvent_PanelCtrl += OnResetProgression;
            _BackButton.onClick.AddListener(BackClickedEvent);
        }

        public void Show(List<Scenario> catalog, List<ChapterProgressionModelDown> userProgression)
        {
            _userProgression = userProgression;
            _scenarioList = catalog;

            _panelController.Show(catalog, userProgression);//Show game panels
            _languageEntryController.Show(catalog);//update Language zone (list of language Toggles)
            _languageCriterias = _languageEntryController.SelectedLanguages;

            _topicEntryController.Init();
            _topicCriteria = _topicEntryController.SelectedTopics;
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

        private void OnFilterTopicPanel(List<string> topics)
        {
            OnFilterTopicPanelAsync(topics);
        }

        private void OnFilterLanguagePanel(List<string> languages)
        {
            _languageCriterias = languages;
            FilterScenarii(_userProgression);
        }

        private void OnFilterTopicPanelAsync(List<string> domains)
        {
            _topicCriteria = domains;
            FilterScenarii(_userProgression);
        }

        private void FilterScenarii(List<ChapterProgressionModelDown> userProgression)
        {
            List<Scenario> filteredScenarioList = new List<Scenario>();

            filteredScenarioList.AddRange(
                _scenarioList.Where(
                    p => p.DomainCodes.Intersect(_topicCriteria).Count() > 0 &&
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
            OnResetProgressionEvent_GameSelectionCtrl?.Invoke(chapterName, scenarioName);
        }

        private void OnDestroy()
        {
            _panelController.OnPanelControllerPlayChapterEvent -= OnPlayChapter;
            _languageEntryController.OnLanguageUpdateEvent -= OnFilterLanguagePanel;
            _panelController.OnResetProgressionEvent_PanelCtrl -= OnResetProgression;
            _BackButton.onClick.RemoveListener(BackClickedEvent);
            OnBackEvent -= BackClickedEvent;
        }
    }
}