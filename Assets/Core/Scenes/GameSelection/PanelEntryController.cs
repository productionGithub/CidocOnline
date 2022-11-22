#define TRACE_ON
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using System.Linq;
using StarterCore.Core.Services.Network;
using Zenject;
using StarterCore.Core.Services.GameState;
using Cysharp.Threading.Tasks;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelEntryController : MonoBehaviour//, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _scenarioTitleTxt;
        [SerializeField] private TextMeshProUGUI _descriptionTxt;

        [SerializeField] private Button _detailButton;
        [SerializeField] private DetailEntryController _detailPanel;

        [SerializeField] private bool _detailToggleState = false;

        [SerializeField] private TagsGroupController _tagsGroup;

        public event Action<string, string> OnPanelEntryControllerPlayEvent;
        public event Action<string, string> OnResetProgressionEvent_PanelEntryCtrl;
        //public event Action OnDetailClickEvent;

        //Scenario title needed when Play chapter is clicked. See OnPlayClicked().
        private string _scenarioTitle;

        private Scenario _scenario;

        public void Init(Scenario scenario)
        {
            Trace.Log("[PanelEntryController] Init!");

            _detailPanel.Init();
            _detailButton.onClick.AddListener(OnDetailButtonClicked);
            _detailPanel.OnResetProgressionEvent_EntryCtrl += ResetProgression;
            _detailPanel.OnDetailEntryControllerPlayEvent += OnPlayClicked;

            _scenario = scenario;
        }

        public void Show(List<Scenario> scenarii, List<ChapterProgressionModelDown> userProgressions)
        {
            Trace.Log("[PanelEntryController] SHOW !");
            _scenarioTitle = _scenario.ScenarioTitle;
            _scenarioTitleTxt.text = $"{_scenario.ScenarioTitle}" + " - [" + $"{_scenario.LanguageTag}" + "]";
            _descriptionTxt.text = $"{_scenario.ScenarioDescription}";

            _detailPanel.Show(_scenario.Chapters, userProgressions);
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

        private void ResetProgression(string chapterName)
        {
            Debug.Log("RESET ! -> PanelEntryController");
            OnResetProgressionEvent_PanelEntryCtrl?.Invoke(chapterName, _scenarioTitle);
        }

        private void OnDestroy()
        {
            _detailButton.onClick.RemoveListener(OnDetailButtonClicked);
            _detailPanel.OnDetailEntryControllerPlayEvent -= OnPlayClicked;
            _detailPanel.OnResetProgressionEvent_EntryCtrl -= ResetProgression;
        }
    }
}
