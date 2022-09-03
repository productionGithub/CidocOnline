using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class DetailEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _chapterTitleTxt;
        [SerializeField] private TextMeshProUGUI _chapterDescriptionTxt;

        [SerializeField] private Button _playButton;

        public event Action<string> OnChapterClickEvent;

        public void Show()
        {
            //Display chapter line info
            _playButton.onClick.AddListener(LoadChapter);
        }

        private void LoadChapter()
        {
            OnChapterClickEvent?.Invoke(_chapterTitleTxt.text);
        }
    }
}
