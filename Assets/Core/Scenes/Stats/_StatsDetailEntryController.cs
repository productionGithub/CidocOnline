using System;
using UnityEngine;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using StarterCore.Core.Services.GameState;
using System.Linq;

namespace StarterCore.Core.Scenes.Stats
{
    public class ChapterLineDetailEntryController : MonoBehaviour
    {
        [SerializeField] private ChapterLineDetailEntry _detailEntryTemplate;
        [SerializeField] private Transform _templateContainer;

        public event Action<string> OnDetailEntryControllerPlayEvent;
        public event Action<string> OnResetProgressionEvent_EntryCtrl;

        private List<ChapterLineDetailEntry> _entriesList;

        List<ChapterProgressionModelDown> _userProgressions;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<ChapterLineDetailEntry>();
            }
            else
            {
                foreach (ChapterLineDetailEntry e in _entriesList)
                {
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
        }

        public void Show(List<ChapterProgressionModelDown> chapters)
        {
            _detailEntryTemplate.gameObject.SetActive(false);

            foreach (ChapterProgressionModelDown chapter in chapters)
            {
                ChapterLineDetailEntry instance = Instantiate(_detailEntryTemplate, _templateContainer);
                _entriesList.Add(instance);

                instance.gameObject.SetActive(true);
                instance.Init();

                int lastChallengeId = 0;

                //Get right progression
                ChapterProgressionModelDown progression = _userProgressions.FirstOrDefault(p => p.ChapterName.ToLower().Equals(chapter.ChapterName.ToLower()));

                if (progression != null)
                {
                    if (progression.LastChallengeId == 1)
                    {
                        lastChallengeId = 0;//When resetting a chapter, lastChallengeID is 1 in DB. We want 0%, not 1/MaxChallengeCount
                    }
                    else
                    {
                        lastChallengeId = (progression.LastChallengeId * 100) / progression.MaxChallengeCount;
                    }
                }

                //instance.Show(chapter);
            }
        }

        public void OnDestroy()
        {

        }
    }
}
