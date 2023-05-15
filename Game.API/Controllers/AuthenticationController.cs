using Game.Contracts.Authentication;
using Game.Core.Common;
using Game.Core.Common.Interfaces.Authentication;
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

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        ITokenService tokenService,
        IUserService userService,
        IAuthenticationService authenticationService)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationResponse> Login(LoginRequest request)
    {
        var response = _authenticationService.Login(request);

        if (response is null)
        {
            return BadRequest("Invalid Credentials.");
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public ActionResult<AuthenticationResponse> Register(RegisterRequest request)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        var response = _authenticationService.Register(request, refreshToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public ActionResult<string> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var id = _userService.GetUserClaimsId();
        var user = InMemory.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User does not exist.");
        }

        if (user.RefreshToken.Equals(refreshToken) is false)
        {
            Console.WriteLine("Refresh token is invalid."); // temporary
            return Unauthorized("Refresh token is invalid.");
        }
        else if (user.TokenExpiry < DateTime.UtcNow)
        {
            Console.WriteLine("Refresh token is expired."); // temporary
            return Unauthorized("Refresh token is invalid");
        }

        var token = _tokenService.GenerateJWT(user);
        _tokenService.GenerateRefreshToken();

        return Ok(token);
    }

    [Authorize]
    [HttpGet("claims")]
    public Guid? GetUserClaimsId()
    {
        var result = _userService.GetUserClaimsId();
        return result;
    }
}
