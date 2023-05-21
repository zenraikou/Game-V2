using System.IdentityModel.Tokens.Jwt;
using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        ITokenService tokenService,
        IUserService userService,
        IAuthenticationService authenticationService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _tokenService = tokenService;
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest request)
    {
        var response = await _authenticationService.Register(request);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var response = await _authenticationService.Login(request);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authenticationService.Logout();
        return Ok();
    }

    // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [HttpGet("refresh-token")]
    // public async Task<ActionResult<AuthenticationResponse>> RefreshToken()
    // {
    //     var cookieRefreshToken = Request.Cookies["refreshToken"];
    //     var refreshToken = await _refreshTokenRepository.Get(t => t.Value == cookieRefreshToken);

    //     if (refreshToken is null)
    //     {
    //         _logger.LogError("Refresh Token is invalid.");
    //         return Unauthorized();
    //     }

    //     if (refreshToken.Expiry < DateTime.UtcNow)
    //     {
    //         _logger.LogError("Current Date is past Refresh Token Expiry");
    //         _logger.LogError($"Refresh Token Expiry Date: {refreshToken.Expiry}");
    //         _logger.LogError($"Current Date: {DateTime.UtcNow}");
    //         return Unauthorized();
    //     }

    //     var user = await _userRepository.Get(u => u.Id == refreshToken.UserId);

    //     if (user is null)
    //     {
    //         return Unauthorized();
    //     }

    //     var jwt = _tokenService.GenerateJWT(user);
    //     var response = new AuthenticationResponse { Token = jwt };

    //     _logger.LogInformation("Current Date is not past Refresh Token Expiry");
    //     _logger.LogInformation($"Refresh Token Expiry Date: {refreshToken.Expiry}");
    //     _logger.LogInformation($"Current Date: {DateTime.UtcNow}");
    //     return Ok(response);
    // }
}
