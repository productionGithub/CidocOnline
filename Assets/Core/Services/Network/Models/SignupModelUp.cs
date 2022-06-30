using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class SignupModelUp
    {
        [JsonProperty("email")]
        public string Email;

        [JsonProperty("password")]
        public string Password;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("optin")]
        public bool Optin;
    }
}