using Newtonsoft.Json;
using StarterCore.Core.Scenes.LeaderBoard;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    //RankingModelDown myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    [Serializable]
    public class RankingModelDown
    {
        [JsonProperty("languages")]
        public List<Language> Languages { get; set; }
    }
}