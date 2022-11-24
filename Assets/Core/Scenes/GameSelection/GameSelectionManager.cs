#define TRACE_ON
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionManager : IInitializable
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private MockNetService _networkService;
        [Inject] private GameSelectionController _gameSelectioncontroller;
        [Inject] private NavigationService _navService;

        ScenariiModelDown _catalog;
        ProgressionModelDown _progression;

        List<ChapterProgressionModelDown> _userProgressions;

        public async void Initialize()
        {
            Debug.Log("[GameSelectionManager] Initialized!");

            //Get scenarii catalog from scenariiCatalog.json file
            _catalog = await _networkService.GetCatalog();

            //string chapterFileName = GetChapterFilename(_gameStateManager.GameStateModel.CurrentChapter);

            //Get user progressions
            _userProgressions = await _networkService.GetUserStats(_gameStateManager.GameStateModel.UserId);

            if (_catalog != null)
            {
                _gameSelectioncontroller.Init();
                _gameSelectioncontroller.Show(_catalog.Scenarii, _userProgressions);
            }
            else
            {
                Debug.LogError("[GameSelectionManager] Could not fetch data from server. Please contact administrator.");
            }

            _gameSelectioncontroller.OnBackEvent += BackEventClicked;
            _gameSelectioncontroller.OnGameSelectionControllerPlayChapterEvent += LoadChapter;
            _gameSelectioncontroller.OnResetProgressionEvent_GameSelectionCtrl += ResetProgression;
        }

        public async void LoadChapter(string scenarioTitle, string chapterTitle)
        {
            _gameStateManager.GameStateModel.CurrentScenario = scenarioTitle;
            _gameStateManager.GameStateModel.CurrentChapter = chapterTitle;

            if (!scenarioTitle.Equals("YOUR GAME HERE!"))
            {
                //Get progression for this scenario-chapter
                _progression = await _networkService.GetChapterProgression(_gameStateManager.GameStateModel.UserId, scenarioTitle, GetChapterFilename(_gameStateManager.GameStateModel.CurrentChapter));

                if(_progression.LastChallengeId != -1)
                {
                    Trace.Log(string.Format("Progression for {0}-{1} is : challenge Id {2} / Score = {3}",
                        scenarioTitle, chapterTitle, _progression.LastChallengeId, _progression.Score));

                    //update GamaModel with values of progression
                    _gameStateManager.GameStateModel.CurrentChallengeIndex = _progression.LastChallengeId;
                    _gameStateManager.GameStateModel.CurrentScore = _progression.Score;
                }
                else
                {
                    string chapterFileName = GetChapterFilename(chapterTitle);
                    await _networkService.CreateSession(_gameStateManager.GameStateModel.UserId, scenarioTitle, chapterFileName);
                    //TODO Debug _session value : always false (but sent is true => Output writtent in error_log).
                    //TODO For now, I don't check and set default values as if _session == true (#everything went fine);

                    _gameStateManager.GameStateModel.CurrentChallengeIndex = 1;
                    _gameStateManager.GameStateModel.CurrentScore = 0;
                }

                _navService.Push("BoardScene");
            }
        }

        private string GetChapterFilename(string chapterName)
        {
            //Get chapter filename property from catalog
            string name = string.Empty;

            //Get filename from catalog
            foreach (Scenario s in _catalog.Scenarii)
            {
                foreach (Chapter c in s.Chapters)
                {
                    if (c.ChapterTitle == chapterName)
                    {
                        name = c.ChapterFilename;
                    }
                }
            }
            return name;
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }

        private async void ResetProgression(string chapterName, string scenarioName)
        {
            //Debug.Log("[GSM] Session data -> " + _gameStateManager.GameStateModel.UserId);
            //Debug.Log("[GSM]Session data -> " + scenarioName);
            //Debug.Log("[GSM]Session data -> " + chapterName);
            //Debug.Log("[GSM]Session data REAL NAME -> " + GetChapterFilename(chapterName));

            ResetProgressionModelUp sessiondata = new ResetProgressionModelUp
            {
                UserId = _gameStateManager.GameStateModel.UserId,
                CurrentScenario = scenarioName,
                CurrentChapter = GetChapterFilename(chapterName)
            };

            _ = await _networkService.ResetProgression(sessiondata);
        }

        private async UniTask<List<ChallengeData>> FetchChallenges(string chapterName)
        {

            Trace.Log("[BoardManager] GetChapters for chapterName " + "->" + chapterName + "<-");

            List<ChallengeData> challenges = await _networkService.LoadChapter(
                _gameStateManager.GameStateModel.CurrentScenario,
                _gameStateManager.GameStateModel.CurrentChapter,
                chapterName);
            Trace.Log("[BoardManager] Got challenges count : " + challenges.Count);
            return challenges;
        }

        //Debug
        private void DebugScenarii()
        {
            foreach (Scenario s in _catalog.Scenarii)
            {
                Debug.Log("Scenar title : " + s.ScenarioTitle);
                Debug.Log("Scenar description : " + s.ScenarioDescription);
                foreach (string ont in s.OntologyTags)
                {
                    Debug.Log("Ontology tags : " + ont);
                }
                foreach (string dom in s.DomainTags)
                {
                    Debug.Log("Ontology tags : " + dom);
                }

                Debug.Log("Language tags : " + s.LanguageTag);

                foreach (Chapter c in s.Chapters)
                {
                    Debug.Log("Chapter title : " + c.ChapterTitle);
                    Debug.Log("Chapter description : " + c.ChapterDescription);
                }
            }
        }

    }
}