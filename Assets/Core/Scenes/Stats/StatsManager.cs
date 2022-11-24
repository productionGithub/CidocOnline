#define TRACE_ON
using System.Collections.Generic;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Services.Navigation;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System;

namespace StarterCore.Core.Scenes.Stats
{
    public class StatsManager : IInitializable
    {
        [Inject] MockNetService _mockNetService;
        [Inject] GameStateManager _gameStateManager;
        [Inject] StatsController _statsController;
        [Inject] NavigationService _navigationService;

        List<ChapterProgressionModelDown> _userStats;

        public async void Initialize()
        {
            Debug.Log("[StatsManager] Show !");
            _userStats = await _mockNetService.GetUserStats(_gameStateManager.GameStateModel.UserId);
            _statsController.Init();
            _statsController.Show(_userStats);

            _statsController.OnMainMenuEvent += MainMenuButtonClicked;
        }

        private void MainMenuButtonClicked()
        {
            _navigationService.Push("MainMenuScene");
        }

        private void OnDestroy()
        {
            _statsController.OnMainMenuEvent -= MainMenuButtonClicked;
        }

    }
}