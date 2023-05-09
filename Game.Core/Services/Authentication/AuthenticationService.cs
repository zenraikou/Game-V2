using Game.Core.Common.Interfaces.Authentication;
using Game.Domain.Entities;

namespace Game.Core.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    public static List<User> users = new();

    private readonly IJWTGenerator _jwtGenerator;

    public AuthenticationService(IJWTGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public AuthenticationResult Login(string uniqueName, string password)
    {
        var user = users.FirstOrDefault(u => u.UniqueName == uniqueName);

        if (user is null) throw new Exception("Invalid Credentials.");

        if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) is false) throw new Exception("Invalid Credentials.");

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

    public AuthenticationResult Register(string handle, string name, string uniqueName, string email, string password)
    {
        var user = users.FirstOrDefault(u => u.UniqueName == uniqueName);

        if (user is not null) throw new Exception("User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        user = new User
        {
            Handle = handle,
            Name = name,
            UniqueName = uniqueName,
            Email = email,
            PasswordHash = passwordHash
        };

        users.Add(user);

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
