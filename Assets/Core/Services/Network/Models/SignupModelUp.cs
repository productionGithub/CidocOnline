using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class SignupModelUp
    {
        [JsonProperty("username")]
        public string Username;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("password")]
        public string Password;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("countryCode")]
        public string CountryCode;

        [JsonProperty("optin")]
        public bool Optin;

        [JsonProperty("lang")]
        public string Lang;
    }
}