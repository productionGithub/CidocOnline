#define TRACE_ON
using System;
using UnityEngine;
using Zenject;

using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network.Models;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.MainMenu
{
    public class MainMenuManager : IInitializable
    {
        [Inject] GameStateManager _gameStateManager;
        [Inject] MainMenuController _mainMenuController;
        [Inject] NavigationService _navigation;
        [Inject] APIService _networkService;

        public async void Initialize()//was async
        {
            Debug.Log("Scene name is " + SceneManager.GetActiveScene().name);
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
            _mainMenuController.OnResetGameEvent += OnResetGame;
            _mainMenuController.OnFullCreditsEvent += OnFullCredits;
            _mainMenuController.OnGoingFurtherEvent += OnGoingFurther;

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
            _navigation.Push("BoardScene");
        }


        private void LoadStatsScreen()
        {
            _navigation.Push("StatsScene");
        }

        private void LoadLeaderBoardScreen()
        {
            _navigation.Push("LeaderBoardScene");
        }

        private void OnQuit()
        {
            _navigation.Push("SigninScene");
        }

        private void OnResetGame()
        {
            _ = _networkService.ResetGame(_gameStateManager.GameStateModel.UserId);
        }

        private void OnFullCredits()
        {
            _navigation.Push("FullCreditsScene");
        }

        private void OnGoingFurther()
        {
            _navigation.Push("GoingFurtherScene");
        }



    }
}