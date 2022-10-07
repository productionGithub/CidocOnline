using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class Chapter
    {
        [JsonProperty("chapter-title")]
        public string ChapterTitle;
        [JsonProperty("chapter-description")]
        public string ChapterDescription;

        public Chapter(string cTitle, string cDescription)
        {
            ChapterTitle = cTitle;
            ChapterDescription = cDescription;
        }
    }
}
