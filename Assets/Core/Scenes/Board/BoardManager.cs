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

namespace StarterCore.Core.Scenes.Board
{
    public class BoardManager : IInitializable
    {
        /// <summary>
        /// BoardManager :
        /// Fetch chapter data
        /// Init BoardController with chapter data
        /// Manage game loop global events such as validate, update gamestate, etc.
        /// </summary>

        [Inject] GameStateManager _gameStateManager;
        [Inject] NavigationService _navigationService;
        [Inject] BoardController _boardController;
        [Inject] MockNetService _networkService;

        ScenariiModelDown _catalog;

        public async void Initialize()
        {
            //Update GameState with current Scenario/Chapter/ChallengeId passed in via NavService bundle
            UpdateGameStateModelWithScenarioData();

            //Get catalog
            _catalog = await GetScenariiCatalog(); 

            //Get chapter filename from catalog
            string chapterFilename = GetChapterFilename();

            //Fetch & Deserialize challenge and instances files
            List<ChallengeData> challenges = await GetCurrentChapter(chapterFilename);
            List<InstanceCardModelDown> instances = await _networkService.GetInstanceFile(_gameStateManager.GameStateModel.CurrentScenario);

            _boardController.Init(challenges, instances);
            _boardController.Show();
        }

        private async UniTask<List<ChallengeData>> GetCurrentChapter(string chapterName)
        {
            List<ChallengeData> chapter = await _networkService.LoadChapter(
                _gameStateManager.GameStateModel.CurrentScenario,
                _gameStateManager.GameStateModel.CurrentChapter,
                chapterName);

            return chapter;
        }

        private async UniTask<ScenariiModelDown> GetScenariiCatalog()
        {
            ScenariiModelDown catalog = await _networkService.GetCatalog();
            return catalog;
        }

        private string GetChapterFilename()
        {
            //Get chapter filename property from catalog
            string name = string.Empty;

            //Get filename from catalog
            foreach (Scenario s in _catalog.Scenarii)
            {
                foreach (Chapter c in s.Chapters)
                {
                    if (c.ChapterTitle == _gameStateManager.GameStateModel.CurrentChapter)
                    {
                        name = c.ChapterFilename;
                    }
                }
            }
            return name;
        }

        private void UpdateGameStateModelWithScenarioData()
        {
            //Get scenario/chapter/challenge index info from NavigationService bundle
            ChallengeInfoBundle bundle = new ChallengeInfoBundle("", "", 1);
            _navigationService.GetMainBundle(out bundle);

            _gameStateManager.GameStateModel.CurrentScenario = bundle.ScenarioTitle;
            _gameStateManager.GameStateModel.CurrentChapter = bundle.ChapterTitle;
            _gameStateManager.GameStateModel.CurrentChallengeIndex = bundle.ChallengeIndex;
            _gameStateManager.GameStateModel.CurrentScore = 0;//TODO : Update with score from Bundle
        }

        private void UpdateChallengeState()// A t this level?
        {
        }

        private void OnGamePaused()
        {
        }

        private void OnAnimateArrow()
        {
        }

        private void OnDestroy()
        {
        }

    }
}