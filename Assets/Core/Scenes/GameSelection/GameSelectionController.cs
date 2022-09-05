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
        [Header("Static")]
        [SerializeField] private Button _BackButton;
        [SerializeField] private PanelController _panelController;
        [SerializeField] private DetailEntryController _detailController;

        public event Action<string, string> OnGameSelectionControllerPlayChapterEvent;
        public event Action OnBackEvent;

        public void Show(List<Scenario> entries)
        {
            _panelController.Show(entries);
            _panelController.OnPanelControllerPlayChapterEvent += OnPlayChapter;
            _BackButton.onClick.AddListener(BackClickedEvent);
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
