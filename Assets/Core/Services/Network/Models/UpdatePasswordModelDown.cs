using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class UpdatePasswordModelDown
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated;
    }
}
