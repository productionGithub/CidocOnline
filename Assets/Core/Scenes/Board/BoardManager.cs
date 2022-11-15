#define TRACE_OFF
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
            //Get catalog
            _catalog = await GetScenariiCatalog(); 

            //Get chapter filename from catalog
            string chapterFilename = GetChapterFilename();

            //Fetch & Deserialize challenge and instances files
            List<ChallengeData> challenges = await GetCurrentChapters(chapterFilename);

            Debug.Log("");

            List<InstanceCardModelDown> instances = await _networkService.GetInstanceFile(_gameStateManager.GameStateModel.CurrentScenario);

            _boardController.Init(challenges, instances);
            _boardController.OnCorrectAnswer_BoardCtrl += UpdateSession;

            _boardController.Show();
        }

        private async void UpdateSession()
        {
            //Update score and current challenge in current progression table
            //Fetch progression table
            //Update lastChallengeId + Score fields + Creation_time
           Trace.Log(string.Format("We update DB with current challenge data : challenge index {0} and score {1}",
                _gameStateManager.GameStateModel.CurrentChallengeIndex, _gameStateManager.GameStateModel.CurrentScore));

            UpdateSessionModelUp session = new UpdateSessionModelUp
            {
                UserId = _gameStateManager.GameStateModel.UserId,
                CurrentScenario = _gameStateManager.GameStateModel.CurrentScenario,
                CurrentChapter = GetChapterFilename(),
                CurrentChallengeIndex = _gameStateManager.GameStateModel.CurrentChallengeIndex,
                CurrentScore = _gameStateManager.GameStateModel.CurrentScore
            };

            bool result = await _networkService.UpdateSession(session);
        }

        private async UniTask<List<ChallengeData>> GetCurrentChapters(string chapterName)
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

        private void UpdateChallengeState()
        {
            //Update BDD with current challenge and score
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