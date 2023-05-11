using Game.Contracts.Authentication;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IAuthenticationService
{
    AuthenticationResponse Login(LoginRequest request);
    AuthenticationResponse Register(RegisterRequest request, RefreshToken refreshToken);
}
