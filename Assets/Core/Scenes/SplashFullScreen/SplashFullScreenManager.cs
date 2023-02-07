using Zenject;
using StarterCore.Core.Services.Navigation;
using UnityEngine;

namespace StarterCore.Core.Scenes.SplashFullScreen
{
    public class SplashFullScreenManager : IInitializable
    {
        [Inject] private SplashFullScreenController _controller;
        [Inject] private NavigationService _navService;

        public void Initialize()
        {
            Debug.Log("SPLASH MANAGER INIT!");
            _controller.OnFullScreenClicked += FullScreenClicked;
            _controller.Init();
        }

        private void FullScreenClicked()
        {
            //Force fullscreen
            _navService.Push("SignInScene");
        }

        private void OnDestroy()
        {
            _controller.OnFullScreenClicked -= FullScreenClicked;
        }
    }
}