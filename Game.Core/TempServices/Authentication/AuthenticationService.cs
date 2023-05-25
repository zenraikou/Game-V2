using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.TempServices.JWT;
using Game.Core.TempServices.PlayerClaim;
using Game.Domain.Entities;

namespace Game.Core.TempServices.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IJWTService _jwtService;
    private readonly IPlayerClaimService _playerClaimService;

    public AuthenticationService(
        IPlayerRepository playerRepository, 
        ISessionRepository sessionRepository, 
        IJWTService jwtService, 
        IPlayerClaimService playerClaimService)
    {
        _playerRepository = playerRepository;
        _sessionRepository = sessionRepository;
        _jwtService = jwtService;
        _playerClaimService = playerClaimService;
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request)
    {
        var player = await _playerRepository.Get(u => u.UniqueName == request.UniqueName);

        if (player is not null) throw new BadRequestException("ID already taken.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var role = (Role)Enum.Parse(typeof(Role), request.Role.Substring(0, 1).ToUpper() + request.Role.Substring(1).ToLower());

        player = new Player
        {
            Handle = request.Handle,
            Name = request.Name,
            UniqueName = request.UniqueName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = role
        };

        await _playerRepository.Post(player);

        var jwt = _jwtService.GenerateJWT(player);
        var session = _jwtService.GenerateSession(jwt);

        await _sessionRepository.Post(session);

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest request)
    {
        var user = await _playerRepository.Get(u => u.UniqueName == request.UniqueName);

        // replace exception with global error handler later
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new BadRequestException("Invalid credentials.");
        }    

        var jwt = _jwtService.GenerateJWT(user);
        var session = _jwtService.GenerateSession(jwt);

        _sessionRepository.Update(session);

        var response = new AuthenticationResponse { JWT = jwt };
        return (response);
    }

    public async Task Logout()
    {
        var jti = _playerClaimService.GetPlayerClaim(c => c.Type == "jti");

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
