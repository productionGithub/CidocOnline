using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class EmailValidationDown
    {
        [JsonProperty("doesExist")]
        public bool DoesExist;
    }
}
