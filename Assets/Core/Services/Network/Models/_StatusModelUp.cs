using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class _StatusModelUp
    {
        [JsonProperty("email")]
        public string Email;
    }
}
