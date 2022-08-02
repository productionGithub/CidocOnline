using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class StatusModelDown
    {
        [JsonProperty("isActive")]
        public bool IsActive;
    }
}
