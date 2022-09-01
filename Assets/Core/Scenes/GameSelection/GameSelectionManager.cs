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


            List<Panel> entriesData = new List<Panel>();

            //Test purpose : data will be fetched from remote server
            for (int i = 0; i <= 9; i++)
            {
                string title = "Abbaye de Marmoutier #" + i.ToString();
                string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris ac tristique enim, vitae sodales elit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Pellentesque rutrum massa mi, id consequat risus feugiat ut. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean tincidunt accumsan nunc vel hendrerit. Vestibulum ut massa risus.";
                Panel panelData = new Panel { GameTitle = title, Description = description };
                entriesData.Add(panelData);
            }

            _controller.Show(entriesData);
            _controller.OnBackEvent += BackEventClicked;
            _controller.OnPanelClickedEvent += PanelClicked;
        }

        private void PanelClicked(string panelName)
        {
            Debug.Log("[GameSelecitonManager] Game Panel named " + panelName + " clicked!. Let's load it");
        }

        private void BackEventClicked()
        {
            Debug.Log("[MANAGER BACKEVET OK!");
            _navService.Pop();
        }
    }
}