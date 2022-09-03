using UnityEngine;
using System;
using Zenject;

using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionController : MonoBehaviour
    {

        //[SerializeField] private GameSelection _template;
        //[SerializeField] private Transform _parent;

        [Header("Static")]
        [SerializeField] private Button _BackButton;
        [SerializeField] private PanelController _panelController;

        public event Action OnBackClickedEvent;
        public event Action<string> OnPanelClickedEvent;

        public event Action OnBackEvent;

        public void Show(List<Panel> entries)
        {

            _panelController.Show(entries);
            _BackButton.onClick.AddListener(BackClickedEvent);
            //TODO Va sauter au profit des details
            _panelController.OnGamePanelClickEvent += PanelClicked;
        }

        private void PanelClicked(string panelName)
        {
            OnPanelClickedEvent?.Invoke(panelName);
        }

        private void BackClickedEvent()
        {
            Debug.Log("BACK TO USSR !");
            OnBackEvent?.Invoke();
        }

        private void OnDestroy()
        {
            OnBackClickedEvent -= BackClickedEvent;
        }
    }
}
