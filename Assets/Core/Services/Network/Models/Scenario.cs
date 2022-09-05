using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class Scenario
    {
        public Scenario(
            string title,
            List<string> ontologyTags,
            List<string> domainTags,
            List<string> languageTags,
            string scenarioDescription,
            List<Chapter> chapters)
        {
            ScenarioTitle = title;
            OntologyTags = new List<string>(ontologyTags);
            DomainTags = new List<string>(domainTags);
            LanguageTags = new List<string>(languageTags);
            ScenarioDescription = scenarioDescription;
            Chapters = new List<Chapter>(chapters);
        }

        [JsonProperty("scenario-title")]
        public string ScenarioTitle;
        [JsonProperty("ontologyTags")]
        public List<string> OntologyTags;
        [JsonProperty("domainTags")]
        public List<string> DomainTags;
        [JsonProperty("languageTags")]
        public List<string> LanguageTags;
        [JsonProperty("scenario-description")]
        public string ScenarioDescription;
        [JsonProperty("chapters")]
        public List<Chapter> Chapters;
        private Scenario scenario;
    }
}
