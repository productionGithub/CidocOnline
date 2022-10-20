using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class XmlStringModelDown
    {
        [JsonProperty("xmlString")]
        public string XmlString;
    }
}