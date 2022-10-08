using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class HistoryModelDown
    {
        [JsonProperty("historyId")]
        public string HistoryId;
        [JsonProperty("scenarioName")]
        public string ScenarioName;
        [JsonProperty("chapterName")]
        public string ChapterName;
        [JsonProperty("challengeId")]
        public string ChallengeId;
        [JsonProperty("score")]
        public string Score;

        public HistoryModelDown(string id, string scenarioName, string chapterName, string challengeId, string score)
        {
            HistoryId = id;
            ScenarioName = scenarioName;
            ChapterName = chapterName;
            ChallengeId = challengeId;
            Score = score;
        }
    }
}


