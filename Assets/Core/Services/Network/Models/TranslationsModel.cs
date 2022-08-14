using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class TranslationsModel
    {
        [JsonProperty("locale")]
        public string Locale;
        [JsonProperty("static-text")]
        public Dictionary<string, Dictionary<string, string>> StaticText;
    }
}


