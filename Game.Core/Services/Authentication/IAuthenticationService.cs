using Game.Contracts.Authentication;

namespace Game.Core.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResponse?> Login(LoginRequest request);
    Task<AuthenticationResponse?> Register(RegisterRequest request);
}
