using TMPro;
using UnityEngine;
using StarterCore.Core.Services.Network.Models;


namespace StarterCore.Core.Scenes.Stats
{
    public class ChapterLineDetailEntry : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI _chapterTitleTxt;
        [SerializeField] public TextMeshProUGUI _chapterProgressionTxt;
        [SerializeField] private TextMeshProUGUI _currentScoreTxt;
        [SerializeField] private TextMeshProUGUI _maxPossibleScoreTxt;

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
    }
}
