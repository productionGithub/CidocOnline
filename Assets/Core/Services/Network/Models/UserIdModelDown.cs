using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class UserIdModelDown
    {
        [JsonProperty("userId")]
        public string UserId;
    }
}