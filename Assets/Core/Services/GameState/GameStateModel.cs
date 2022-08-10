using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.GameState
{
    [Serializable]
    public class GameStateModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }
    }
}