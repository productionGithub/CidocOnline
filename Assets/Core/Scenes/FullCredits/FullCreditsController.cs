using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.FullCredits
{
    public class FullCreditsController : MonoBehaviour
    {
        [SerializeField] private Button _BackButton;

        public event Action OnBackEvent;

        public void Init()
        {
            _BackButton.onClick.AddListener(OnBackClicked);
        }

        private void OnBackClicked()
        {
            OnBackEvent?.Invoke();
        }

        private void OnDestroy()
        {
            _BackButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}


