using System;

[Serializable]
public class RegisterResponse
{
    public string IdToken; //A Firebase Auth ID token for the newly created user.
    public string Email; //The email for the newly created user
    public string RefreshToken; //A Firebase Auth refresh token for the newly created user.
    public string ExpiresIn; //The number of seconds in which the ID token expires.
    public string LocalId; //The uid of the newly created user.
}
