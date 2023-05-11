using System.Security.Cryptography;
using Game.Contracts.Authentication;
using Game.Core.Common;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Game.Domain.Entities;
using Game.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Game.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly IDateTImeProvider _dateTimeProvider;
    private readonly JWTSettings _jwtSettings;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IAuthenticationService authenticationService,
        IUserService userService,
        IDateTImeProvider dateTimeProvider,
        IOptions<JWTSettings> jwtSettings)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationResponse> Login(LoginRequest request)
    {
        var response = _authenticationService.Login(request);
        return Ok(response);
    }

    [HttpPost("register")]
    public ActionResult<AuthenticationResponse> Register(RegisterRequest request)
    {
        var refreshToken = GenerateRefreshToken();
        var response = _authenticationService.Register(request, refreshToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var uniqueName = _userService.GetCurrentUserUniqueName();
        var user = InMemory.Users.FirstOrDefault(u => u.UniqueName == uniqueName);

        if (user.RefreshToken.Equals(refreshToken))
        {
            
        }

        return Ok("string");
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expiry = _dateTimeProvider.Now.AddHours(_jwtSettings.Expiry)
        };

        return refreshToken;
    }

    public void SetRefreshToken(RefreshToken refreshedToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshedToken.Expiry
        };

        Response.Cookies.Append("refreshToken", refreshedToken.Token, cookieOptions);

        InMemory.Users.FirstOrDefault();
    }
}
