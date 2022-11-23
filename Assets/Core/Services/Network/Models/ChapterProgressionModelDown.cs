using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChapterProgressionModelDown
    {
        [JsonProperty("scenarioName")]
        public string ScenarioName { get; set; }
        [JsonProperty("scenarioLanguage")]
        public string ScenarioLanguage { get; set; }
        [JsonProperty("chapterName")]
        public string ChapterName { get; set; }
        [JsonProperty("lastChallengeId")]
        public int LastChallengeId { get; set; }
        [JsonProperty("maxChallengeCount")]
        public int MaxChallengeCount { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("maxPossibleScore")]
        public int MaxPossibleScore { get; set; }
    }
}


/*
 * 
 *         [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("scenarioName")]
        public string ScenarioName { get; set; }
        [JsonProperty("lastChapterName")]
        public string LastChapterName { get; set; }
        [JsonProperty("lastChallengeId")]
        public int LastChallengeId { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
*/






/*
 * 
 * 

*/