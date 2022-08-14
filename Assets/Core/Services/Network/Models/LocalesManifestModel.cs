using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class LocalesManifestModel
    {
        [JsonProperty("locales")]
        public Dictionary<string, string> Locales;
    }
}

