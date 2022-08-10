using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class InstanceCardModelDown
    {
        [JsonProperty("Id")]
        public string Id;
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Label")]
        public string Label;
        [JsonProperty("ImageName")]
        public string ImageName;
    }
}