using Newtonsoft.Json;
using System;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ResetPasswordModelDown
    {
        [JsonProperty("emailSent")]
        public bool EmailSent;
    }
}
