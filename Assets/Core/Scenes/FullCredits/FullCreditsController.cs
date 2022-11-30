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
            Debug.Log("FC CONTROLLER INIT!");
            _BackButton.onClick.AddListener(OnBackClicked);
        }

        private void OnBackClicked()
        {
            Debug.Log("Back btn event ok");
            OnBackEvent?.Invoke();
        }

        private void OnDestroy()
        {
            _BackButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}


