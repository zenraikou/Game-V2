using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
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

        if (user is null) return null;

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is false) return null;

        var token = _tokenService.GenerateJWT(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        await _tokenService.RefreshToken(user.Id, refreshToken);

        var response = new AuthenticationResponse { Token = token };
        return (response);
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request, RefreshToken refreshToken)
    {
        var user = await _userRepository.Get(u => u.UniqueName == request.UniqueName);

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

        await _userRepository.Post(user);

        var response = await Login(new LoginRequest { UniqueName = request.UniqueName, Password = request.Password });
        return response;
    }
}
