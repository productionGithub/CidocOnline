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
using StarterCore.Core.Scenes.GameSelection;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private GameSelectionController _gameSelectioncontroller;
        [Inject] private NavigationService _navService;

        ScenariiModelDown _catalog;

        public void Initialize()
        {
            Debug.Log("[GameSelectionManager] Initialized!");
            //List<Scenario> scenarioList = new List<Scenario>();

            //Get scenarii catalog and Show Game selection panel
            FetchCatalog().Forget();

            _gameSelectioncontroller.OnBackEvent += BackEventClicked;
            _gameSelectioncontroller.OnGameSelectionControllerPlayChapterEvent += LoadChapter;
        }

        private void LoadChapter(string scenarioTitle, string chapterTitle)
        {
            //Update GameState with current Scenario / Chapter

            ChapterInfoBundle bundle = new ChapterInfoBundle(scenarioTitle, chapterTitle);
            _navService.Push("Board", bundle);
            Debug.Log(string.Format("[GameSelectionManager] Load chapter{0} of scenario {1}", chapterTitle, scenarioTitle));
        }

        //private async UniTaskVoid FetchCatalog()
        private async UniTaskVoid FetchCatalog()
        {
            _catalog = await GetScenariiCatalog();

            if(_catalog != null)
            {
                _gameSelectioncontroller.Show(_catalog.Scenarii);
                //DebugScenarii();
            }
        }

        private async UniTask<ScenariiModelDown> GetScenariiCatalog()
        {
            ScenariiModelDown catalog = await _net.GetCatalog();
            return catalog;
        }

        private void BackEventClicked()
        {
            _navService.Pop();
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