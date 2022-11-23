using System;
using UnityEngine;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using StarterCore.Core.Services.GameState;
using System.Linq;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DetailEntryController : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;

        [SerializeField] private DetailEntry _detailEntryTemplate;
        [SerializeField] private Transform _templateContainer;

        public event Action<string> OnDetailEntryControllerPlayEvent;
        public event Action<string> OnResetProgressionEvent_EntryCtrl;

        private List<DetailEntry> _entriesList;

        List<ChapterProgressionModelDown> _userProgressions;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<DetailEntry>();
            }
            else
            {
                foreach (DetailEntry e in _entriesList)
                {
                    e.OnDetailEntryPlayEvent -= OnPlayChapterClicked;
                    e.OnResetProgressionEvent_Entry -= OnResetProgression;
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
        }

        public void Show(List<Chapter> chapters, List<ChapterProgressionModelDown> userProgressions)
        {
            _detailEntryTemplate.gameObject.SetActive(false);

            foreach (Chapter chapter in chapters)
            {
                DetailEntry instance = Instantiate(_detailEntryTemplate, _templateContainer);
                _entriesList.Add(instance);

                instance.gameObject.SetActive(true);
                instance.Init();

                int lastChallengeId=0;

                //Get right progression
                ChapterProgressionModelDown progression = userProgressions.FirstOrDefault(p => p.ChapterName.ToLower().Equals(chapter.ChapterTitle.ToLower()));

                if (progression != null)
                {
                    if(progression.LastChallengeId == 1)
                    {
                        lastChallengeId = 0;//When resetting a chapter, lastChallengeID is 1 in DB. We want 0%, not 1/MaxChallengeCount
                    }
                    else
                    {
                        lastChallengeId = (progression.LastChallengeId * 100) / progression.MaxChallengeCount;
                    }
                }

                instance.Show(chapter, lastChallengeId);

                instance.OnDetailEntryPlayEvent += OnPlayChapterClicked;
                instance.OnResetProgressionEvent_Entry += OnResetProgression;
            }
        }

        private void OnPlayChapterClicked(string chapterName)
        {
            OnDetailEntryControllerPlayEvent?.Invoke(chapterName);
        }

        private void OnResetProgression(string chapterName)
        {
            //Reset UI for progression to [0];
            DetailEntry thisEntry = _entriesList.FirstOrDefault(e => e._chapterTitleTxt.text.Equals(chapterName));
            if(thisEntry)
            {
                thisEntry._chapterProgressionTxt.text = "0%";
            }

            OnResetProgressionEvent_EntryCtrl?.Invoke(chapterName);
        }

        public void OnDestroy()
        {
            
        }
    }
}
