using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.GoingFurther
{
    public class GoingFurtherController : MonoBehaviour
    {
        [SerializeField] private Button _CidocGameButton;
        [SerializeField] private Button _YoutubeButton;
        [SerializeField] private Button _CidocCRMButton;
        [SerializeField] private Button _BackButton;

        public event Action OnCidocGameEvent;
        public event Action OnYoutubeEvent;
        public event Action OnCidocCrmEvent;
        public event Action OnBackEvent;

        public void Init()
        {
            Debug.Log("GF Controller INIT!");
            _CidocGameButton.onClick.AddListener(OnCidocGame);
            _YoutubeButton.onClick.AddListener(OnYoutube);
            _CidocCRMButton.onClick.AddListener(OnCidocCrm);
            _BackButton.onClick.AddListener(OnBackClicked);
        }

        private void OnBackClicked()
        {
            Debug.Log("Back btn event ok");
            OnBackEvent?.Invoke();
        }

        private void OnCidocGame()
        {
            OnCidocGameEvent?.Invoke();
        }

        private void OnYoutube()
        {
            OnYoutubeEvent?.Invoke();
        }

        private void OnCidocCrm()
        {
            OnCidocCrmEvent?.Invoke();
        }

        private void OnDestroy()
        {
            _BackButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}


