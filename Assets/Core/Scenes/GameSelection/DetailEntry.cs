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

        [SerializeField] private Button _playChapterButton;

        public event Action<string> OnDetailEntryPlayEvent;

        public void Show(Chapter chapter)
        {
            _playChapterButton.onClick.AddListener(LoadChapter);

            _chapterTitleTxt.text = $"{chapter.ChapterTitle}";
            _chapterDescriptionTxt.text = $"{chapter.ChapterDescription}";
        }

        private void LoadChapter()
        {
            OnDetailEntryPlayEvent?.Invoke(_chapterTitleTxt.text);
        }
    }
}
