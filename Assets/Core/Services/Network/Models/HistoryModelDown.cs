using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class HistoryModelDown
    {
        [JsonProperty("scenarioName")]
        public string ScenarioName;
        [JsonProperty("chapterName")]
        public string ChapterName;
        [JsonProperty("challengeId")]
        public string ChallengeId;
        [JsonProperty("score")]
        public string Score;

        public HistoryModelDown(
            string scenarioName,
            string chapterName,
            string challengeId,
            string score)
        {
            ScenarioName = scenarioName;
            ChapterName = chapterName;
            ChallengeId = challengeId;
            Score = score;
        }
    }
}
