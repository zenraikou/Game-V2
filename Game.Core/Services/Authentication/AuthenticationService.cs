using Game.Core.Common.Interfaces.Authentication;
using Game.Domain.Entities;

namespace Game.Core.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJWTGenerator _jwtGenerator;

    public AuthenticationService(IJWTGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public AuthenticationResult Login(string email, string password)
    {
        var result = new AuthenticationResult
        {
            Handle = "handle",
            Name = "name",
            UniqueName = "uniqueName",
            Email = email,
            Token = "token"
        };

        return result;
    }

    public AuthenticationResult Register(string handle, string name, string uniqueName, string email, string password)
    {
        // Check if user(email) already exists

        // use BCrypt to hash password

        var user = new User
        {
            Handle = handle,
            Name = name,
            UniqueName = uniqueName,
            Email = email,
            PasswordHash = password
        };

        var token = _jwtGenerator.GenerateToken(user.Id, user.Name, user.UniqueName, user.Email);
        
        var result = new AuthenticationResult
        {
            Id = user.Id,
            Handle = user.Handle,
            Name = user.Name,
            UniqueName = user.UniqueName,
            Email = user.Email,
            Token = token
        };
        
        return result;
    }
}
