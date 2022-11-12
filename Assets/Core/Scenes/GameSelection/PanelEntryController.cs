#define TRACE_ON
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using Zenject;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelEntryController : MonoBehaviour//, IPointerClickHandler
    {
        [Inject] private MockNetService _networkService;
        [Inject] private GameStateManager _gameStateManager;

        [SerializeField] private TextMeshProUGUI _scenarioTitleTxt;
        [SerializeField] private TextMeshProUGUI _descriptionTxt;

        [SerializeField] private Button _detailButton;
        [SerializeField] private DetailEntryController _detailPanel;

        [SerializeField] private bool _detailToggleState = false;

        [SerializeField] private TagsGroupController _tagsGroup;

        public event Action<string, string> OnPanelEntryControllerPlayEvent;

        public event Action OnDetailClickEvent;

        //Scenario title needed when Play chapter is clicked. See OnPlayClicked().
        private string _scenarioTitle;

        ChapterCompletionModelDown _completions;

        private Scenario _scenario;

        //public void Init(Scenario scenario, ChapterCompletionModelDown c)
        public void Init(Scenario scenario)
        {
            Trace.Log("[PanelEntryController] Init!");
            _scenario = scenario;
            //_completions = c;
        }

        public void Show()
        {
            Trace.Log("[PanelEntryController] SHOW !");
            _scenarioTitle = _scenario.ScenarioTitle;

            _detailButton.onClick.AddListener(OnDetailButtonClicked);

            _scenarioTitleTxt.text = $"{_scenario.ScenarioTitle}" + " - [" + $"{_scenario.LanguageTag}" + "]";
            _descriptionTxt.text = $"{_scenario.ScenarioDescription}";


            //Detail panel
            //_detailPanel.Show(_scenario.Chapters, _completions);
            _detailPanel.Show(_scenario.Chapters);

            _detailPanel.OnDetailEntryControllerPlayEvent += OnPlayClicked;

            //TagsGroup
            _tagsGroup.Show(_scenario.DomainTags, _scenario.OntologyTags, _scenario.AuthorTags);
        }

        private void OnPlayClicked(string chapterName)
        {
            OnPanelEntryControllerPlayEvent?.Invoke(_scenarioTitle, chapterName);
        }

        private void OnDetailButtonClicked()
        {

            _detailToggleState = !_detailToggleState;
            _detailPanel.gameObject.SetActive(_detailToggleState);
        }

        //private async UniTask<ChapterCompletionsModelDown> GetChapterCompletions()
        //{
        //    Trace.Log("[PanelEntryController] GETCHAPTERCOMPLETIONS !");
        //    ChapterCompletionsModelDown c = await _networkService.GetChapterCompletions(_gameStateManager.UserId, _scenario.ScenarioTitle);
        //    return c;
        //}

    }
}
