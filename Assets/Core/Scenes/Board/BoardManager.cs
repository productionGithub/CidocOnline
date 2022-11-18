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
            Debug.Log("[BoardManager] AFTER PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentChapter);
            _catalog = await _networkService.GetCatalog();
            Debug.Log("[BoardManager] AFTER PUSH BOARDSCENE : " + _gameStateManager.GameStateModel.CurrentChapter);


            //Get chapter filename from catalog
            string chapterFilename = GetChapterFilename();

            Trace.Log(string.Format("[BoardManager] AFTER GetFilename CurrentChapter is ", _gameStateManager.GameStateModel.CurrentChapter));

            Debug.Log("[BoardManager] GETCHAPTer filenAME = " + chapterFilename);

            //Fetch & Deserialize challenge and instances files
            List<ChallengeData> challenges = await FetchChallenges(chapterFilename);

            List<InstanceCardModelDown> instances = await _networkService.GetInstanceFile(_gameStateManager.GameStateModel.CurrentScenario);

            _boardController.Init(challenges, instances);
            _boardController.InitBoard(challenges, instances);
            _boardController.OnUpdateSession += UpdateSession;

            _boardController.Show();
        }

        private async void UpdateSession()
        {
            Trace.Log(string.Format("We update DB with current challenge data : challenge index {0} and score {1}",
    _gameStateManager.GameStateModel.CurrentChallengeIndex, _gameStateManager.GameStateModel.CurrentScore));

            //Update score and current challenge in current progression table
            //Fetch progression table
            //Update lastChallengeId + Score fields + Creation_time


            UpdateSessionModelUp session = new UpdateSessionModelUp
            {
                UserId = _gameStateManager.GameStateModel.UserId,
                CurrentScenario = _gameStateManager.GameStateModel.CurrentScenario,
                CurrentChapter = GetChapterFilename(),
                CurrentChallengeIndex = _gameStateManager.GameStateModel.CurrentChallengeIndex,
                CurrentScore = _gameStateManager.GameStateModel.CurrentScore
            };

            Debug.Log("[GSM] Session data UserId -> " + _gameStateManager.GameStateModel.UserId);
            Debug.Log("[GSM]Session data Scenar -> " + _gameStateManager.GameStateModel.CurrentScenario);
            Debug.Log("[GSM]Session data ChapterFileName-> " + GetChapterFilename());
            Debug.Log("[GSM]Session data ChallengeIndex -> " + _gameStateManager.GameStateModel.CurrentChallengeIndex);
            Debug.Log("[GSM]Session data Score -> " + _gameStateManager.GameStateModel.CurrentScore);

            Debug.Log("");

            bool result = await _networkService.UpdateSession(session);
        }

        private async UniTask<List<ChallengeData>> FetchChallenges(string chapterName)
        {

            Trace.Log("[BoardManager] GetChapters for chapterName " + "->"+chapterName+"<-");

            List<ChallengeData> challenges = await _networkService.LoadChapter(
                _gameStateManager.GameStateModel.CurrentScenario,
                _gameStateManager.GameStateModel.CurrentChapter,
                chapterName);
            Trace.Log("[BoardManager] Got challenges count : " + challenges.Count);
            return challenges;
        }

        private async UniTask<ScenariiModelDown> GetScenariiCatalog()
        {
            ScenariiModelDown catalog = await _networkService.GetCatalog();
            return catalog;
        }

        private string GetChapterFilename()
        {
            Debug.Log("[BoardManager] game model CurrentChapter is " + _gameStateManager.GameStateModel.CurrentChapter);
            //Get chapter filename property from catalog
            string name = string.Empty;

            //Get filename from catalog
            foreach (Scenario s in _catalog.Scenarii)
            {
                foreach (Chapter c in s.Chapters)
                {
                    Debug.Log("[BoardManager] CTitle ->" + c.ChapterTitle + "<-");
                    Debug.Log("[BoardManager] CModel ->" + _gameStateManager.GameStateModel.CurrentChapter + "<-");

                    if (c.ChapterTitle.ToLower().Equals(_gameStateManager.GameStateModel.CurrentChapter.ToLower()))
                    {
                        Debug.Log("[BoardManager] FOUND -> " + c.ChapterTitle);
                        name = c.ChapterFilename;
                    }
                }
            }
            return name;
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