#define TRACE_ON
using System.Collections;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LeaderBoardDisplayer : MonoBehaviour
    {
        public Transform _leaderBoardContainer;

        public void Init()
        {
        }

        // Start is called before the first frame update
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
