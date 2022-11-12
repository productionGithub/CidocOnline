using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class SessionObjMapping
    {
        [JsonProperty("sessionId")]
        public int SessionId;
        [JsonProperty("historyId")]
        public int HistoryId;
        [JsonProperty("scenarioId")]
        public int ScenarioId;
        [JsonProperty("lastChapterId")]
        public int LastChapterId;
        [JsonProperty("creation_date")]
        public int Creation_date;


        public SessionObjMapping(
            int sessionId,
            int historyId,
            int scenarioId,
            int lastChapterId,
            int creation_date)
        {
            SessionId = sessionId;
            HistoryId = historyId;
            ScenarioId = scenarioId;
            LastChapterId = lastChapterId;
            Creation_date = creation_date;
        }
    }
}