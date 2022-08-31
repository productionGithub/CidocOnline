using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class Panel
    {
        [JsonProperty("gameTitle")]
        public string GameTitle;

        [JsonProperty("description")]
        public string Description;
    }
}
