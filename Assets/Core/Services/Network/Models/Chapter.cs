using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    /// <summary>
    /// This class is used by GameSelection to display the catalog in Panels.
    /// </summary>
    [Serializable]
    public class Chapter
    {
        [JsonProperty("chapter-title")]
        public string ChapterTitle;
        [JsonProperty("chapter-description")]
        public string ChapterDescription;
        [JsonProperty("chapter-filename")]
        public string ChapterFilename;
    }
}
