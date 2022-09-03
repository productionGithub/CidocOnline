using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelEntryController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private DetailEntry _detailEntry;

        [SerializeField] private TextMeshProUGUI _scenarioTitleTxt;
        [SerializeField] private TextMeshProUGUI _descriptionTxt;

        [SerializeField] private Button _detailButton;
        [SerializeField] private GameObject _detailPanel;

        [SerializeField] private bool _detailToggleState = false;

        public event Action<string> OnPanelClickEvent;
        public event Action OnDetailClickEvent;

        public void Show(Panel panel)
        {
            _detailButton.onClick.AddListener(OnToggleDetail);
            _scenarioTitleTxt.text = $"{panel.GameTitle}";
            _descriptionTxt.text = $"{panel.Description}";
            Debug.Log("[GamePanel Initialized!");
        }

        private void OnToggleDetail()
        {
            _detailToggleState = !_detailToggleState;
            _detailPanel.gameObject.SetActive(_detailToggleState);
            Debug.Log("Scenario Title text ref is " + _scenarioTitleTxt.text);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("[Panel Entry] Panel clicked!");
            OnPanelClickEvent?.Invoke(_scenarioTitleTxt.text);
        }
    }
}

