using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarterCore.Core.Scenes.GameSelection
{

    public class GameSelection : MonoBehaviour
    {
        [SerializeField] private Button _BackButton;

        public event Action OnBackClickedEvent;

        public void Show()
        {
            Debug.Log("[GameSelection Initialized!");

            _BackButton.onClick.AddListener(() => OnBackClickedEvent?.Invoke());
        }
    }
}