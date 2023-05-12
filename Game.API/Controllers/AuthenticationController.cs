using Game.Contracts.Authentication;
using Game.Core.Common;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly IDateTImeProvider _dateTimeProvider;
    private readonly IJWTGenerator _jwtGenerator;
    private readonly ITokenRefresher _jwtRefresher;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IAuthenticationService authenticationService,
        IUserService userService,
        IDateTImeProvider dateTimeProvider,
        IJWTGenerator jwtGenerator,
        ITokenRefresher jwtRefresher)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
        _jwtGenerator = jwtGenerator;
        _jwtRefresher = jwtRefresher;
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationResponse> Login(LoginRequest request)
    {
        var response = _authenticationService.Login(request);

        if (response is null)
        {
            return BadRequest("Invalid Credentials.");
        }

        // var refreshToken = GenerateRefreshToken();
        // SetRefreshToken(refreshToken, response.Value.Item2);

        return Ok(response);
    }

    [HttpPost("register")]
    public ActionResult<AuthenticationResponse> Register(RegisterRequest request)
    {
        var refreshToken = _jwtRefresher.RefreshToken();
        var response = _authenticationService.Register(request, refreshToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public ActionResult<string> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var uniqueName = _userService.GetCurrentUserUniqueName();
        var user = InMemory.Users.FirstOrDefault(u => u.UniqueName == "uniqueName");

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

        var token = _jwtGenerator.GenerateToken(user);
        _jwtRefresher.RefreshToken();

        return Ok(token);
    }

    [Authorize]
    [HttpGet("claims")]
    public ActionResult<List<string>> GetUserClaims()
    {
        var result = _userService.GetCurrentUserUniqueName();

        // var user = InMemory.Users.FirstOrDefault(u => u.Email == result);

        // if (user is null)
        // {
        //     return BadRequest("Invalid user claims.");
        // }

        return Ok(result);
    }
}
