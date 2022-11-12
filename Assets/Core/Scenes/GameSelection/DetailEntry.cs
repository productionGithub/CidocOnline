using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using StarterCore.Core.Services.Network.Models;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DetailEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _chapterTitleTxt;
        [SerializeField] private TextMeshProUGUI _chapterDescriptionTxt;
        [SerializeField] private TextMeshProUGUI _chapterCompletionTxt;

        [SerializeField] private Button _playChapterButton;

        public event Action<string> OnDetailEntryPlayEvent;

        //public void Show(Chapter chapter, int completionRate)
        public void Show(Chapter chapter)
        {
            _playChapterButton.onClick.AddListener(PlayChapter);

            _chapterTitleTxt.text = $"{chapter.ChapterTitle}";
            _chapterDescriptionTxt.text = $"{chapter.ChapterDescription}";

            _chapterCompletionTxt.text = "TODO";// completionRate.ToString();
        }

        private void PlayChapter()
        {
            OnDetailEntryPlayEvent?.Invoke(_chapterTitleTxt.text);
        }
    }
}
