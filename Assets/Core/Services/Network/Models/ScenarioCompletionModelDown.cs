using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ScenarioCompletionModelDown
    {
        [JsonProperty("completions")]
        public List<ChapterCompletionModelDown> Completions { get; set; }
    }
}
