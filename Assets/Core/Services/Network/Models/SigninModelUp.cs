using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    public class SigninModelUp
    {
        [JsonProperty("email")]
        public string Email;

        [JsonProperty("password")]
        public string Password;
    }
}