using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChapterCompletionModelDown
    {
        [JsonProperty("chapterName")]
        public string ChapterName { get; set; }
        [JsonProperty("challengeIndex")]
        public List<int> ChallengeIndex { get; set; }
    }
}
