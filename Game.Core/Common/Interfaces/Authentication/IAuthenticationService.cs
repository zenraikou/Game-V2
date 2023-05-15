using Game.Contracts.Authentication;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResponse?> Login(LoginRequest request);
    Task<AuthenticationResponse?> Register(RegisterRequest request, RefreshToken refreshToken);
}
