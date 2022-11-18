using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class ResetProgressionModelUp
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("currentScenario")]
        public string CurrentScenario;

        [JsonProperty("currentChapter")]
        public string CurrentChapter;
    }
}