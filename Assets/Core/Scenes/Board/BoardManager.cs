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
        [Inject] MockNetService _netService;

        ScenariiModelDown _catalog;
        ScenariiModelDown _katalog;

        public async void Initialize()
        {
            //Update GameState with current Scenario/Chapter/ChallengeId passed in via NavService bundle
            UpdateGameStateModelWithScenarioData();

            //Get catalog
            _katalog = await GetScenariiCatalog(); 

            //Get chapter filename from catalog
            string chapterFilename = GetChapterFilename();

            //Fetch & Deserialize challenge file content (all challenges of this chapter)
            List<ChallengeData> chapterChallenges = await GetKurrentChapter(chapterFilename);

            //Show boardController with challenges data
            _boardController.Show(chapterChallenges);
        }

        private async UniTask<List<ChallengeData>> GetKurrentChapter(string chapterName)
        {
            List<ChallengeData> chapter = await _netService.LoadChapter(
                _gameStateManager.GameStateModel.CurrentScenario,
                _gameStateManager.GameStateModel.CurrentChapter,
                chapterName);

            return chapter;
        }

        private async UniTask<ScenariiModelDown> GetScenariiCatalog()
        {
            ScenariiModelDown catalog = await _netService.GetCatalog();
            Debug.Log("[BoardManager] CATALGO ===> " + catalog.Scenarii[0].Chapters[0].ChapterFilename);
            return catalog;
        }

        private string GetChapterFilename()
        {
            //Get chapter filename property from catalog
            string name = string.Empty;

            //Get filename from catalog
            foreach (Scenario s in _katalog.Scenarii)
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