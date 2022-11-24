using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.GameState;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.Stats
{
    public class ChapterLineDetailEntry : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI _chapterTitleTxt;
        [SerializeField] public TextMeshProUGUI _chapterProgressionTxt;
        [SerializeField] private TextMeshProUGUI _currentScoreTxt;
        [SerializeField] private TextMeshProUGUI _maxPossibleScoreTxt;

        public void Init()
        {
        }

        //public void Show(Chapter chapter, int completionRate)
        public void Show(ChapterProgressionModelDown chapter)
        {
            _chapterTitleTxt.text = $"{chapter.ChapterName}";
            if (chapter.LastChallengeId == 1)
            {
                _chapterProgressionTxt.text = "0%";//When resetting a chapter, lastChallengeID is 1 in DB. We want 0%, not 1/MaxChallengeCount
            }
            else
            {
                _chapterProgressionTxt.text = chapter.LastChallengeId * 100 / chapter.MaxChallengeCount + "%";
            }
            _currentScoreTxt.text = $"{chapter.Score}";
            _maxPossibleScoreTxt.text = $"{chapter.MaxPossibleScore}";
        }

        public void OnDestroy()
        {
        }
    }
}
