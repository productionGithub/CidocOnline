using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.GameState;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DetailEntry : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI _chapterTitleTxt;
        [SerializeField] private TextMeshProUGUI _chapterDescriptionTxt;
        [SerializeField] public TextMeshProUGUI _chapterProgressionTxt;

        [SerializeField] private Button _playChapterButton;
        [SerializeField] private Button _resetProgressionButton;

        public event Action<string> OnDetailEntryPlayEvent;
        public event Action<string> OnResetProgressionEvent_Entry;

        public void Init()
        {
            _playChapterButton.onClick.AddListener(PlayChapter);
            _resetProgressionButton.onClick.AddListener(ResetProgression);
        }
        //public void Show(Chapter chapter, int completionRate)
        public void Show(Chapter chapter, int lastChallengeId)
        {
            _chapterTitleTxt.text = $"{chapter.ChapterTitle}";
            _chapterDescriptionTxt.text = $"{chapter.ChapterDescription}";

            //_chapterProgressionTxt.text = "[" + lastChallengeId.ToString() + "]";
            _chapterProgressionTxt.text = lastChallengeId.ToString() + "%";
        }

        private void PlayChapter()
        {
            OnDetailEntryPlayEvent?.Invoke(_chapterTitleTxt.text);
        }

        private void ResetProgression()
        {
            OnResetProgressionEvent_Entry?.Invoke(_chapterTitleTxt.text);
        }

        public void OnDestroy()
        {
            _playChapterButton.onClick.RemoveListener(PlayChapter);
            _resetProgressionButton.onClick.RemoveListener(ResetProgression);
        }
    }
}
