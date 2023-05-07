namespace Game.Core.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    public AuthenticationResult Login(string email, string password)
    {
        var result = new AuthenticationResult
        {
            Name = "name",
            Handle = "handle",
            Email = email,
            Token = "token"
        };

        return result;
    }

    public AuthenticationResult Register(string name, string handle, string email, string password)
    {
        var result = new AuthenticationResult
        {
            Name = name,
            Handle = handle,
            Email = email,
            Token = "token"
        };
        
        return result;
    }
}
