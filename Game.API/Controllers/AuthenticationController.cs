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

    [HttpPost("refresh-token"), Authorize]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var cookieRefreshToken = Request.Cookies["refreshToken"];

        var id = _userService.GetUserClaimsId();
        var user = await _userRepository.Get(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User does not exist.");
        }

        var refreshToken = await _refreshTokenRepository.Get(user.Id);

        if (refreshToken.Token.Equals(cookieRefreshToken) is false)
        {
            Console.WriteLine("Refresh token is invalid."); // temporary
            return Unauthorized("Refresh token is invalid.");
        }
        else if (refreshToken.Expiry < DateTime.UtcNow)
        {
            // Console.WriteLine("Refresh token is expired."); // temporary
            Console.WriteLine("Current Date is past Refresh Token Expiry");
            Console.WriteLine($"Refresh Token Expiry Date: {refreshToken.Expiry}"); // temporary
            Console.WriteLine($"Current Date: {DateTime.UtcNow}"); // temporary
            return Unauthorized("Refresh token is invalid");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshedToken = _tokenService.GenerateRefreshToken(user.Id);

        _refreshTokenRepository.Update(refreshedToken);

        Console.WriteLine("Current Date is not past Refresh Token Expiry");
        Console.WriteLine($"Refresh Token Expiry Date: {refreshToken.Expiry}"); // temporary
        Console.WriteLine($"Current Date: {DateTime.UtcNow}"); // temporary
        return Ok(accessToken);
    }

    [HttpGet("claims"), Authorize]
    public Guid? GetUserClaimsId()
    {
        var result = _userService.GetUserClaimsId();
        return result;
    }
}
