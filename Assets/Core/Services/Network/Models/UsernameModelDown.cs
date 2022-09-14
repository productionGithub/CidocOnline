using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class UsernameModelDown
    {
        [JsonProperty("username")]
        public string Username;
    }
}