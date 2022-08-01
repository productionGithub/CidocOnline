using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ActivationCodeModelDown
    {
        [JsonProperty("code")]
        public string Code;
    }
}