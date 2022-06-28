using System;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class UserProfile
    {
        [JsonProperty("GameName")]
        public string Gamename;

        [JsonProperty("Email")]
        public string Email;

        [JsonProperty("Password")]
        public string Password;

        [JsonProperty("Country")]
        public string Country;

        [JsonProperty("Optin")]
        public string Optin;

        //public UserProfile(string name, string text1, string text2, string text3, bool isOn)
        //{
        //    gamename = name;
        //    Email = text1;
        //    Password = text2;
        //    Country = text3;
        //    Optin = isOn.ToString();
        //}
    }
}
