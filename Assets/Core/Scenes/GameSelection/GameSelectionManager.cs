#define TRACE_ON
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Services.GameState;
using System.Collections.Generic;
using System;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionManager : IInitializable
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private MockNetService _networkService;
        [Inject] private GameSelectionController _gameSelectioncontroller;
        [Inject] private NavigationService _navService;

        ScenariiModelDown _catalog;
        ScenarioCompletionModelDown _completion;
        ProgressionModelDown _progression;

        public void Initialize()
        {
            Debug.Log("[GameSelectionManager] Initialized!");

            //Get scenarii catalog and Show Game selection panel
            FetchScenariiData().Forget();

            _gameSelectioncontroller.OnBackEvent += BackEventClicked;
            _gameSelectioncontroller.OnGameSelectionControllerPlayChapterEvent += LoadChapter;
            _gameSelectioncontroller.OnResetProgressionEvent_GameSelectionCtrl += ResetProgression;
        }

        //TODO TRACK DOWN DATA => Chapter data sent to server is not good
        public async void LoadChapter(string scenarioTitle, string chapterTitle)
        {
            //Debug.Log("[GSM] Scenar -> " + scenarioTitle);
            //Debug.Log("[GSM] Chapter -> " + chapterTitle);

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

                    //update GmaeModel with values of progression
                    _gameStateManager.GameStateModel.CurrentChallengeIndex = _progression.LastChallengeId;
                    _gameStateManager.GameStateModel.CurrentScore = _progression.Score;
                }
                else
                {
                    //Create Session + Default Progression in DB.
                    //Update GameModel with default values for score and challende index.
                    Trace.Log("No progression, create session and load with defaults");
                    Trace.Log("BEFORE CREATING SESSION");
                    string chapterFileName = GetChapterFilename(chapterTitle);
                    Debug.Log("[GSM]" + chapterFileName);

                    bool _session = await _networkService.CreateSession(_gameStateManager.GameStateModel.UserId, scenarioTitle, chapterFileName);

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

        private async UniTaskVoid FetchScenariiData()
        {
            _catalog = await _networkService.GetCatalog();
            if (_catalog != null)
            {
                _gameSelectioncontroller.Init();
                _gameSelectioncontroller.Show(_catalog.Scenarii);//Plus progression ?
                //DebugScenarii();
            }
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }

        private async void ResetProgression(string chapterName, string scenarioName)
        {
            Debug.Log("[GSM] Session data -> " + _gameStateManager.GameStateModel.UserId);
            Debug.Log("[GSM]Session data -> " + scenarioName);
            Debug.Log("[GSM]Session data -> " + chapterName);
            Debug.Log("[GSM]Session data REAL NAME -> " + GetChapterFilename(chapterName));

            ResetProgressionModelUp sessiondata = new ResetProgressionModelUp
            {
                UserId = _gameStateManager.GameStateModel.UserId,
                CurrentScenario = scenarioName,
                CurrentChapter = GetChapterFilename(chapterName)
            };

            _ = await _networkService.ResetProgression(sessiondata);
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