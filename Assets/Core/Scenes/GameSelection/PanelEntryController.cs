using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;

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
        
        public event Action OnDetailClickEvent;

        //Scenario title needed when Play chapter is clicked. See OnPlayClicked().
        private string _scenarioTitle;


        public void Show(Scenario scenario)
        {
            _scenarioTitle = scenario.ScenarioTitle;

            _detailButton.onClick.AddListener(OnDetailButtonClicked);

            _scenarioTitleTxt.text = $"{scenario.ScenarioTitle}" + " - [" + $"{scenario.LanguageTag}" + "]";
            _descriptionTxt.text = $"{scenario.ScenarioDescription}";


            //Detail panel
            _detailPanel.Show(scenario.Chapters);
            _detailPanel.OnDetailEntryControllerPlayEvent += OnPlayClicked;

            //TagsGroup
            _tagsGroup.Show(scenario.DomainTags, scenario.OntologyTags, scenario.AuthorTags);
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
    }
}
