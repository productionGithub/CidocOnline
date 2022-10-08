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

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("currentScenario")]
        public string CurrentScenario;

        [JsonProperty("currentChapter")]
        public string CurrentChapter;

        [JsonProperty("currentChallenge")]
        public int CurrentChallenge;

        [JsonProperty("currentScore")]
        public int CurrentScore;
    }
}
