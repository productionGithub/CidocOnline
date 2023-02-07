using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.SplashFullScreen
{
    public class SplashFullScreenController : MonoBehaviour
    {
        [SerializeField] private Button _FullScreenButton;

        public event Action OnFullScreenClicked;

        public void Init()
        {
            _FullScreenButton.onClick.AddListener(OnBackClicked);
        }

        private void OnBackClicked()
        {
            OnFullScreenClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _FullScreenButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}

