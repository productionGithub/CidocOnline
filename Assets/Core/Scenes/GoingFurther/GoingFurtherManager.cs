using Zenject;
using StarterCore.Core.Services.Navigation;
using UnityEngine;

namespace StarterCore.Core.Scenes.GoingFurther
{
    public class GoingFurtherManager : IInitializable
    {
        [Inject] private GoingFurtherController _controller;
        [Inject] private NavigationService _navService;

        public void Initialize()
        {
            Debug.Log("GF MANAGER INIT!");
            _controller.OnCidocGameEvent += CidocGameClicked;
            _controller.OnYoutubeEvent += OnYoutubeClicked;
            _controller.OnCidocCrmEvent += CidocCrmClicked;
            _controller.OnBackEvent += BackEventClicked;
            _controller.Init();
        }

        private void CidocGameClicked()
        {
            Application.OpenURL("https://www.cidoc-crm-game.org/");
        }

        private void OnYoutubeClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=JJX-nszOVbA");
        }

        private void CidocCrmClicked()
        {
            Application.OpenURL("http://www.cidoc-crm.org/");
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}