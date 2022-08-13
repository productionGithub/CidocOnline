using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Localization.Models
{
    public class Translations
    {
        [JsonProperty("locale")]
        public string Locale;
        [JsonProperty("static-toxt")]
        public Dictionary<string, Dictionary<string, string>> StaticText;
    }
}


