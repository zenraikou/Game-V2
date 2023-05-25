using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.TempServices.Token;
using Game.Core.TempServices.UserClaim;
using Game.Domain.Entities;

namespace Game.Core.TempServices.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserClaimService _userClaimService;

    public AuthenticationService(
        IUserRepository userRepository, 
        ISessionRepository sessionRepository, 
        ITokenService tokenService, 
        IUserClaimService userClaimService)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _tokenService = tokenService;
        _userClaimService = userClaimService;
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request)
    {
        var user = await _userRepository.Get(u => u.UniqueName == request.UniqueName);

        if (user is not null) throw new BadRequestException("ID already taken.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var role = (Role)Enum.Parse(typeof(Role), request.Role.Substring(0, 1).ToUpper() + request.Role.Substring(1).ToLower());

        user = new User
        {
            Handle = request.Handle,
            Name = request.Name,
            UniqueName = request.UniqueName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = role
        };

        await _userRepository.Post(user);

        var jwt = _tokenService.GenerateJWT(user);
        var session = _tokenService.GenerateSession(jwt);

        await _sessionRepository.Post(session);

        var response = new AuthenticationResponse { Token = jwt };
        return response;
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest request)
    {
        var user = await _userRepository.Get(u => u.UniqueName == request.UniqueName);

        // replace exception with global error handler later
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new BadRequestException("Invalid credentials.");
        }    

        var jwt = _tokenService.GenerateJWT(user);
        var session = _tokenService.GenerateSession(jwt);

        _sessionRepository.Update(session);

        var response = new AuthenticationResponse { Token = jwt };
        return (response);
    }

    public async Task Logout()
    {
        var jti = _userClaimService.GetUserClaim(c => c.Type == "jti");

        // replace exception with global error handler later
        if (jti is null)
        {
            throw new Exception("JTI is null.");
        }

        var session = await _sessionRepository.Get(s => s.JTI == jti);

        // replace exception with global error handler later
        if (session is null)
        {
            throw new Exception("Session is null.");
        }

        _sessionRepository.Delete(session);
    }
}
