#define TRACE_OFF
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LeaderBoardDisplayer : MonoBehaviour
    {
        public Transform _leaderBoardContainer;

        public void ShowLanguageTitle(LanguageTitleEntry title)
        {
            title.gameObject.SetActive(true);
        }

        public void ShowLanguageTitleDivider(LanguageTitleDivider divider)
        {
            divider.gameObject.SetActive(true);
        }

        public void ShowScenarioTitle(ScenarioData data)
        {
            data.gameObject.SetActive(true);
        }

    }
}
