using System;
using Newtonsoft.Json;


namespace CidocOnline2022.Core.Services.Network.Models
{
    [Serializable]
    public class User
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("password")]
        public string Password;

        [JsonProperty("first_name")]
        public string FirstName;

        [JsonProperty("last_name")]
        public string LastName;

        [JsonProperty("email")]
        public string Email;
    }
}
