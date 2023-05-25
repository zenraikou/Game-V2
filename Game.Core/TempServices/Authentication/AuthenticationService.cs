using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.TempServices.JWT;
using Game.Core.TempServices.PlayerClaim;
using Game.Domain.Entities;

namespace Game.Core.TempServices.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;
    private readonly IPlayerClaimService _playerClaimService;

    public AuthenticationService(IUnitOfWork unitOfWork, IJWTService jwtService, IPlayerClaimService playerClaimService)
    {
        _jwtService = jwtService;
        _playerClaimService = playerClaimService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request)
    {
        var player = await _unitOfWork.Players.Get(u => u.UniqueName == request.UniqueName);
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

        await _unitOfWork.Players.Post(player);

        var jwt = _jwtService.GenerateJWT(player);
        var session = _jwtService.GenerateSession(jwt);

        await _unitOfWork.Sessions.Post(session);

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest request)
    {
        var user = await _unitOfWork.Players.Get(u => u.UniqueName == request.UniqueName);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) throw new BadRequestException("Invalid credentials.");

        var jwt = _jwtService.GenerateJWT(user);
        var session = _jwtService.GenerateSession(jwt);

        await _unitOfWork.Sessions.Update(session);

        var response = new AuthenticationResponse { JWT = jwt };
        return (response);
    }

    public async Task Logout()
    {
        var jti = _playerClaimService.GetPlayerClaim(c => c.Type == "jti");
        if (jti is null) throw new UnauthorizedException("Invalid access.");

        var session = await _unitOfWork.Sessions.Get(s => s.JTI == jti);
        if (session is null) throw new UnauthorizedException("Invalid access.");

        await _unitOfWork.Sessions.Delete(session);
    }
}
