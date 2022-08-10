using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class _UpdatePasswordModelDown
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated;
    }
}
