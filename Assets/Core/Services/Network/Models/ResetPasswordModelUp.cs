using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class ResetPasswordModelUp
    {
        [JsonProperty("email")]
        public string Email;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("lang")]
        public string Lang;
    }
}