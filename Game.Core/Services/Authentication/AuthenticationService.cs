using Game.Contracts.Authentication;
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

    public AuthenticationResponse Login(LoginRequest request)
    {
        var user = users.FirstOrDefault(u => u.UniqueName == request.UniqueName);

        if (user is null) throw new Exception("Invalid Credentials.");

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is false) throw new Exception("Invalid Credentials.");

        var token = _jwtGenerator.GenerateToken(user);

        var response = new AuthenticationResponse { Token = token };

        return response;
    }

    public AuthenticationResponse Register(RegisterRequest request)
    {
        var user = users.FirstOrDefault(u => u.UniqueName == request.UniqueName);

        if (user is not null) throw new Exception("User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        user = new User
        {
            Handle = request.Handle,
            Name = request.Name,
            UniqueName = request.UniqueName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        users.Add(user);

        var token = _jwtGenerator.GenerateToken(user);

        var response = new AuthenticationResponse { Token = token };

        return response;
    }
}
