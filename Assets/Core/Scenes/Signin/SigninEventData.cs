using System;

[Serializable]
public class SigninEventData
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    public SigninEventData(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
