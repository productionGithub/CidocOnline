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
    }
}