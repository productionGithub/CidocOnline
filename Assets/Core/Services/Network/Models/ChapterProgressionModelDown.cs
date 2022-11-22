using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChapterProgressionModelDown
    {
        [JsonProperty("scenarioName")]
        public string ScenarioName { get; set; }
        [JsonProperty("lastChapterName")]
        public string LastChapterName { get; set; }
        [JsonProperty("lastChallengeId")]
        public int LastChallengeId { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
    }
}