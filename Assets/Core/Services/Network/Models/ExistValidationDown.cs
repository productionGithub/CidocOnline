using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ExistValidationDown
    {
        [JsonProperty("doesExist")]
        public bool DoesExist;
    }
}
