using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class Scenario
    {
        [JsonProperty("scenario-title")]
        public string ScenarioTitle;
        [JsonProperty("scenario-description")]
        public string ScenarioDescription;
        [JsonProperty("chapters")]
        public List<Chapter> Chapters;
    }
}