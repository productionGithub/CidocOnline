using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.GameState
{
    [Serializable]
    public class GameStateModel
    {
        [JsonProperty("locale")]
        public string Locale;

        [JsonProperty("default-locale")]
        public string DefaultLocale;

        [JsonProperty("username")]
        public string Username;

        [JsonProperty("userId")]
        public int UserId;//int

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("currentScenario")]
        public string CurrentScenario;

        [JsonProperty("currentChapter")]
        public string CurrentChapter;

        [JsonProperty("currentChallengeIndex")]
        public int CurrentChallengeIndex;

        [JsonProperty("currentScore")]
        public int CurrentScore;
    }
}
