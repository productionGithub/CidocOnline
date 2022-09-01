using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelEntry : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _gameTitleTxt;
        [SerializeField] private TextMeshProUGUI _descriptionTxt;

        [SerializeField] private Button _detailButton;
        [SerializeField] private GameObject _detailPanel;
        private bool detailToggleState = false;

        public event Action<string> OnClickEvent;


        public void Show(Panel panel)
        {
            _detailButton.onClick.AddListener(ToggleDetail);
            _gameTitleTxt.text = $"{panel.GameTitle}";
            _descriptionTxt.text = $"{panel.Description}";
            Debug.Log("[GamePanel Initialized!");
        }

        private void ToggleDetail()
        {
            detailToggleState = !detailToggleState;
            _detailPanel.gameObject.SetActive(detailToggleState);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke(_gameTitleTxt.text);
        }

    }
}