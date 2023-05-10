using Game.Contracts.Authentication;

namespace Game.Core.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResponse Login(LoginRequest request);
    AuthenticationResponse Register(RegisterRequest request);
}
