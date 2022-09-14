using System;

[Serializable]
public class SignupEventData
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Country { get; private set; }
    public string CountryCode { get; private set; }
    public bool Optin { get; private set; }

    public SignupEventData(string username, string email, string password, string country, string countryCode, bool optin)
    {
        Username = username;
        Email = email;
        Password = password;
        Country = country;
        CountryCode = countryCode;
        Optin = optin;
    }
}
