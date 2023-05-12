using Game.Contracts.Authentication;
using Game.Core.Common;
using Game.Core.Common.Interfaces.Authentication;
using Game.Domain.Entities;

namespace Game.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJWTGenerator _jwtGenerator;
    private readonly ITokenRefresher _jwtRefreshser;

    public AuthenticationService(IJWTGenerator jwtGenerator, ITokenRefresher jwtRefreshser)
    {
        _jwtGenerator = jwtGenerator;
        _jwtRefreshser = jwtRefreshser;
    }

    public AuthenticationResponse? Login(LoginRequest request)
    {
        var user = InMemory.Users.FirstOrDefault(u => u.UniqueName == request.UniqueName);

        if (user is null) return null;

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is false) return null;

        var token = _jwtGenerator.GenerateToken(user);
        var refreshToken = _jwtRefreshser.RefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.TokenExpiry = refreshToken.Expiry;
        user.TokenCreationStamp = refreshToken.CreationStamp;

        var response = new AuthenticationResponse { Token = token };
        return (response);
    }

    public AuthenticationResponse Register(RegisterRequest request, RefreshToken refreshToken)
    {
        var user = InMemory.Users.FirstOrDefault(u => u.UniqueName == request.UniqueName);

        if (user is not null) throw new Exception("User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var role = (Role)Enum.Parse(typeof(Role), request.Role.Substring(0, 1).ToUpper() + request.Role.Substring(1).ToLower());

        user = new User
        {
            Handle = request.Handle,
            Name = request.Name,
            UniqueName = request.UniqueName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = role,
            RefreshToken = refreshToken.Token,
            TokenExpiry = refreshToken.Expiry,
            TokenCreationStamp = refreshToken.CreationStamp
        };

        InMemory.Users.Add(user);

        var token = _jwtGenerator.GenerateToken(user);
        var response = new AuthenticationResponse { Token = token };
        return response;
    }
}
