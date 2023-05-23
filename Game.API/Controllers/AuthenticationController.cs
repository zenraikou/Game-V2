using Game.API.Attributes;
using Game.Contracts.Authentication;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting]
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
}
