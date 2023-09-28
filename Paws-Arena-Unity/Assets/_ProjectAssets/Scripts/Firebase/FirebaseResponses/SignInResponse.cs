using System;

[Serializable]
public class SignInResponse
{
    public string IdToken;
    public string Email;
    public string RefreshToken;
    public string ExpiresIn;
    public string LocalId;
    public bool Registered;
}
