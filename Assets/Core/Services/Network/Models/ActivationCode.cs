using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ActivationCode
    {
        [JsonProperty("code")]
        public string Activationcode;
    }
}