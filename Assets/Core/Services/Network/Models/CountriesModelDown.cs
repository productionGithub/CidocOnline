using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class CountriesModelDown
    {
        [JsonProperty("countries")]
        public List<CountryDictionary> Countries { get; set; }
    }

    [Serializable]
    public class CountryDictionary
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
