using Zenject;
using StarterCore.Core.Services.Navigation;
using UnityEngine;

namespace StarterCore.Core.Scenes.FullCredits
{
    public class FullCreditsManager : IInitializable
    {
        [Inject] private FullCreditsController _controller;
        [Inject] private NavigationService _navService;

        public void Initialize()
        {
            Debug.Log("FC MANAGER INIT!");
            _controller.OnBackEvent += BackEventClicked;
            _controller.Init();
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}