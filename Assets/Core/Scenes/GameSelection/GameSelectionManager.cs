using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Localization;


namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionManager : IInitializable
    {
        //[Inject] private MockNetService _net;
        [Inject] private GameSelectionController _controller;
        [Inject] private NavigationService _navService;
        //[Inject] private GameStateManager _gameState;

        public void Initialize()
        {
            Debug.Log("[GameSelectionManager] Initialized!");
            _controller.Show();
            _controller.OnBackEvent += BackEventClicked;
        }

        private void BackEventClicked()
        {
            Debug.Log("[MANAGER BACKEVET OK!");
            _navService.Pop();
        }
    }
}