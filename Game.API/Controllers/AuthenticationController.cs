using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        ITokenService tokenService,
        IUserService userService,
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userService = userService;
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthenticationResponse>> RefreshToken()
    {
        var cookieRefreshToken = Request.Cookies["refreshToken"];
        var refreshToken = await _refreshTokenRepository.Get(t => t.Value == cookieRefreshToken);

        if (refreshToken is null)
        {
            _logger.LogError("Refresh Token is invalid.");
            return Unauthorized();
        }

        if (refreshToken.Expiry < DateTime.UtcNow)
        {
            _logger.LogError("Current Date is past Refresh Token Expiry");
            _logger.LogError($"Refresh Token Expiry Date: {refreshToken.Expiry}");
            _logger.LogError($"Current Date: {DateTime.UtcNow}");
            return Unauthorized();
        }

        var user = await _userRepository.Get(u => u.Id == refreshToken.UserId);

        if (user is null)
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var response = new AuthenticationResponse { Token = accessToken };

        _logger.LogInformation("Current Date is not past Refresh Token Expiry");
        _logger.LogInformation($"Refresh Token Expiry Date: {refreshToken.Expiry}");
        _logger.LogInformation($"Current Date: {DateTime.UtcNow}");
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var response = await _authenticationService.Login(request);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest request)
    {
        var response = await _authenticationService.Register(request);
        return Ok(response);
    }

    [HttpGet("claims"), Authorize]
    public Guid? GetUserClaimsId()
    {
        var result = _userService.GetUserClaimsId();
        return result;
    }
}
