using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class GameModelDown
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("domainTags")]
        public List<string> DomainTags { get; set; }
        [JsonProperty("ontologyTags")]
        public List<string> OntologyTags { get; set; }
        [JsonProperty("languageTags")]
        public List<string> LanguageTags { get; set; }
        [JsonProperty("challengeList")]
        public List<ChallengeData> ChallengeList { get; set; }
    }
}
