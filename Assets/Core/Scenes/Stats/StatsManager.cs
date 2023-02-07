#define TRACE_OFF
using System.Collections.Generic;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.Stats
{
    public class StatsManager : IInitializable
    {
        [Inject] APIService _mockNetService;
        [Inject] GameStateManager _gameStateManager;
        [Inject] StatsController _statsController;
        [Inject] NavigationService _navigationService;

        List<ChapterProgressionModelDown> _userStats;

        public async void Initialize()
        {
            _statsController.ShowWaitingIcon();
            _userStats = await _mockNetService.GetUserStats(_gameStateManager.GameStateModel.UserId);
            _statsController.HideWaitingIcon();

            _statsController.Init();
            _statsController.OnMainMenuEvent += MainMenuButtonClicked;

            _statsController.Show(_userStats);
        }

        private void MainMenuButtonClicked()
        {
            _navigationService.Push("MainMenuScene");
        }

        public void OnDestroy()
        {
            _statsController.OnMainMenuEvent -= MainMenuButtonClicked;
        }

    }
}