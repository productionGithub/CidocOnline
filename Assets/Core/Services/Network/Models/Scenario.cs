using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class Scenario
    {
        [JsonProperty("scenario-title")]
        public string ScenarioTitle;
        [JsonProperty("ontologyTags")]
        public List<string> OntologyTags;
        [JsonProperty("domainTags")]
        public List<string> DomainTags;
        [JsonProperty("authorTags")]
        public List<string> AuthorTags;
        [JsonProperty("languageTag")]
        public string LanguageTag;
        [JsonProperty("scenario-description")]
        public string ScenarioDescription;
        [JsonProperty("chapters")]
        public List<Chapter> Chapters;
        //private Scenario scenario;

        public Scenario(
            string title,
            List<string> ontologyTags,
            List<string> domainTags,
            List<string> authorTags,
            string languageTag,
            string scenarioDescription,
            List<Chapter> chapters)

        {
            ScenarioTitle = title;
            OntologyTags = new List<string>(ontologyTags);
            DomainTags = new List<string>(domainTags);
            AuthorTags = new List<string>(authorTags);
            LanguageTag = languageTag;
            ScenarioDescription = scenarioDescription;
            Chapters = new List<Chapter>(chapters);
        }


    }
}
