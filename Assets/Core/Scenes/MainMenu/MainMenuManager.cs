#define TRACE_ON
using System;
using UnityEngine;
using Zenject;

using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.MainMenu
{
    public class MainMenuManager : IInitializable
    {
        [Inject] GameStateManager _gameStateManager;
        [Inject] MainMenuController _mainMenuController;
        [Inject] NavigationService _navigation;
        [Inject] MockNetService _networkService;

        public async void Initialize()//was async
        {
            //Get player history
            _mainMenuController.ShowWaitingIcon();
            HistoryModelDown history = await _networkService.GetHistory(_gameStateManager.GameStateModel.UserId);
            _mainMenuController.HideWaitingIcon();

            if (!history.ScenarioName.Equals(string.Empty))
            {
                //Update game state model
                _gameStateManager.GameStateModel.CurrentScenario = history.ScenarioName;
                _gameStateManager.GameStateModel.CurrentChapter = history.ChapterName;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = Int32.Parse(history.ChallengeId);
                _gameStateManager.GameStateModel.CurrentScore = Int32.Parse(history.Score);
            }
            else
            {
                _gameStateManager.GameStateModel.CurrentScenario = string.Empty;
                _gameStateManager.GameStateModel.CurrentChapter = string.Empty;
                _gameStateManager.GameStateModel.CurrentChallengeIndex = 0;
                _gameStateManager.GameStateModel.CurrentScore = 0;
            }

            //Events
            _mainMenuController.OnContinueChapterEvent += ContinueChapter;
            _mainMenuController.OnChooseScenarioEvent += LoadGameSelectionScreen;
            _mainMenuController.OnStatisticsEvent += LoadStatsScreen;
            _mainMenuController.OnLeaderBoardEvent += LoadLeaderBoardScreen;
            _mainMenuController.OnQuitEvent += OnQuit;

            Show();
        }

        public void Show()
        {
            _mainMenuController.Init();
            _mainMenuController.Show();
        }

        private void LoadGameSelectionScreen()
        {
            _navigation.Push("GameSelectionScene");
        }

        private void ContinueChapter()
        {
            //Debug.Log("[MainMenuManager] Before PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentScenario);
            //Debug.Log("[MainMenuManager] Before PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentChapter);
            //Debug.Log("[MainMenuManager] Before PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentChallengeIndex);
            //Debug.Log("[MainMenuManager] Before PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentScore);
            _navigation.Push("BoardScene");
        }


        private void LoadStatsScreen()
        {
            _navigation.Push("StatsScene");
        }

        private void LoadLeaderBoardScreen()
        {
            Trace.Log("PUSH LEADERBOARD SCENE!!!!");
            _navigation.Push("LeaderBoardScene");
        }

        private void OnQuit()
        {
            _navigation.Push("SigninScene");
        }
    }
}