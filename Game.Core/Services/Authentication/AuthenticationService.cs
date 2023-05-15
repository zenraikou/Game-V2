using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Authentication;
using Game.Domain.Entities;

namespace Game.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest request)
    {
        var user = await _userRepository.Get(u => u.UniqueName == request.UniqueName);

        // replace exception with global error handler later
        if (user is null) throw new Exception("Bad Request: Invalid credentials.");

        // replace exception with global error handler later
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) throw new Exception("Bad Request: Invalid credentials.");

        var token = _tokenService.GenerateJWT(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.TokenExpiry = refreshToken.Expiry;
        user.TokenCreationStamp = refreshToken.CreationStamp;

        var response = new AuthenticationResponse { Token = token };
        return (response);
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request)
    {
        var user = await _userRepository.Get(u => u.UniqueName == request.UniqueName);

        if (user is not null) throw new Exception("Bad Request: User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var role = (Role)Enum.Parse(typeof(Role), request.Role.Substring(0, 1).ToUpper() + request.Role.Substring(1).ToLower());
        var refreshToken = _tokenService.GenerateRefreshToken();

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

        await _userRepository.Post(user);

        var token = _tokenService.GenerateJWT(user);
        var response = new AuthenticationResponse { Token = token };
        return response;
    }
}
