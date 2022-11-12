using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ProgressionModelDown
    {
        [JsonProperty("LastChallengeId")]
        public int LastChallengeId;
        [JsonProperty("Score")]
        public int Score;


        public ProgressionModelDown(int challengeId, int score)
        {
            LastChallengeId = challengeId;
            Score = score;
        }
    }
}


