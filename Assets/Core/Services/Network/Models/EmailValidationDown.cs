using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class EmailValidationDown
    {
        [JsonProperty("isValid")]
        public string IsValid;
    }
}
