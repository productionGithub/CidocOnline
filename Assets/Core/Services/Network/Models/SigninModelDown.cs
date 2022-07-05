using Newtonsoft.Json;
using System;


namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class SigninModelDown
    {
        [JsonProperty("login")]
        public bool LoginResult;
    }
}
