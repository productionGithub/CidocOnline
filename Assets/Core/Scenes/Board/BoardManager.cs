#define TRACE_OFF
using System.Collections.Generic;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardManager : IInitializable
    {
        [Inject] GameStateManager _gameStateManager;
        [Inject] BoardController _boardController;
        [Inject] APIService _networkService;

        ScenariiModelDown _catalog;

        public async void Initialize()
        {
            Debug.Log("Scene name is " + SceneManager.GetActiveScene().name);
            _catalog = await _networkService.GetCatalog();

            //Get chapter filename from catalog
            string chapterFilename = GetChapterFilename();

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
            //Update score and current challenge in current progression table
            //Update lastChallengeId + Score fields + Creation_time

            UpdateSessionModelUp session = new UpdateSessionModelUp
            {
                UserId = _gameStateManager.GameStateModel.UserId,
                CurrentScenario = _gameStateManager.GameStateModel.CurrentScenario,
                CurrentChapter = GetChapterFilename(),
                CurrentChallengeIndex = _gameStateManager.GameStateModel.CurrentChallengeIndex,
                CurrentScore = _gameStateManager.GameStateModel.CurrentScore
            };

            //Debug.Log("[GSM] Session data UserId -> " + _gameStateManager.GameStateModel.UserId);
            //Debug.Log("[GSM]Session data Scenar -> " + _gameStateManager.GameStateModel.CurrentScenario);
            //Debug.Log("[GSM]Session data ChapterFileName-> " + GetChapterFilename());
            //Debug.Log("[GSM]Session data ChallengeIndex -> " + _gameStateManager.GameStateModel.CurrentChallengeIndex);
            //Debug.Log("[GSM]Session data Score -> " + _gameStateManager.GameStateModel.CurrentScore);

            _ = await _networkService.UpdateSession(session);
        }

        private async UniTask<List<ChallengeData>> FetchChallenges(string chapterName)
        {
            List<ChallengeData> challenges = await _networkService.LoadChapter(
                _gameStateManager.GameStateModel.CurrentScenario,
                _gameStateManager.GameStateModel.CurrentChapter,
                chapterName);
            return challenges;
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
                    //Debug.Log("[BoardManager] CTitle ->" + c.ChapterTitle + "<-");
                    //Debug.Log("[BoardManager] CModel ->" + _gameStateManager.GameStateModel.CurrentChapter + "<-");

                    if (c.ChapterTitle.ToLower().Equals(_gameStateManager.GameStateModel.CurrentChapter.ToLower()))
                    {
                        //Debug.Log("[BoardManager] FOUND -> " + c.ChapterTitle);
                        name = c.ChapterFilename;
                    }
                }
            }
            return name;
        }

        public void OnDestroy()
        {
            _boardController.OnUpdateSession -= UpdateSession;
        }

    }
}