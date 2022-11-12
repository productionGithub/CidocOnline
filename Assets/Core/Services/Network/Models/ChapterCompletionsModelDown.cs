using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChapterCompletionModelDown
    {
        [JsonProperty("completions")]
        public List<int> Completions { get; set; }
    }
}
