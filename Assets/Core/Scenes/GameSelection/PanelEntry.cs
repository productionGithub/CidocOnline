using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class PanelEntry : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _gameTitleTxt;
        [SerializeField] private TextMeshProUGUI _descriptionTxt;

        public event Action<string> OnClickEvent;


        public void Show(Panel panel)
        {
            _gameTitleTxt.text = $"{panel.GameTitle}";
            _descriptionTxt.text = $"{panel.Description}";
            Debug.Log("[GameSelection Initialized!");
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke(_gameTitleTxt.text);
        }

    }
}