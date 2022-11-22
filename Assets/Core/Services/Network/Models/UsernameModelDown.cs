using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class UserNameModelDown
    {
        [JsonProperty("username")]
        public string Username;
    }
}