using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace StarterCore.Core.Scenes.GameSelection
{

    public class GameSelection : MonoBehaviour
    {
        [SerializeField] private Button _BackButton;
        [SerializeField] private GameObject _thisGameLine;
        public event Action OnBackClickedEvent;
        public event Action OnGameClickedEvent;

        public void Show()
        {
            Debug.Log("[GameSelection Initialized!");
            _BackButton.onClick.AddListener(() => OnBackClickedEvent?.Invoke());
        }
    }
}