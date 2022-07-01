using System;

[Serializable]
public class SignupEventData
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Country { get; private set; }
    public bool Optin { get; private set; }

    public SignupEventData(string email, string password, string country, bool optin)
    {
        Email = email;
        Password = password;
        Country = country;
        Optin = optin;
    }
}
