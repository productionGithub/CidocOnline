using System;
using UnityEngine;
using Zenject;

using StarterCore.Core.Services.Navigation;

namespace StarterCore.Core.Scenes.MainMenu
{
    public class MainMenuManager : IInitializable
    {
        [Inject] MainMenuController _mainMenuController;
        [Inject] NavigationService _navigation;

        public void Initialize()
        {
            Trace.Log("[MainMenuManager] Initialized!");
            _mainMenuController.Show();

            _mainMenuController.OnChooseScenarioEvent += LoadGameSelectionScreen;
        }

        private void LoadGameSelectionScreen()
        {
            _navigation.Push("GameSelectionScene");
        }
    }
}