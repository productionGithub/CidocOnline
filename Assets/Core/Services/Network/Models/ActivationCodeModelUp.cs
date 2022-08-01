using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ActivationCodeModelUp
    {
        [JsonProperty("email")]
        public string Email;
    }
}