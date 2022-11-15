using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class CreateSessionModelUp
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("scenarioTitle")]
        public string ScenarioTitle;

        [JsonProperty("chapterTitle")]
        public string ChapterTitle;
    }
}
