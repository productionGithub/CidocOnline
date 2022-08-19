using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class User
    {
        [JsonProperty("HistoryID")]
        public int HistoryID;

        [JsonProperty("Username")]
        public string Username;

        [JsonProperty("Password")]
        public string Password;

        [JsonProperty("Country")]
        public string Country;

        [JsonProperty("CountryCode")]
        public string CountryCode;

        [JsonProperty("Email")]
        public string Email;
    }
}
