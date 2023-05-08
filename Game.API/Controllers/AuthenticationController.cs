using Game.Contracts.Authentication;
using Game.Core.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authenticationService.Login(request.Email, request.Password);

        var response = new AuthenticationResponse
        {
            Handle = result.Handle,
            Name = result.Name,
            UniqueName = result.UniqueName,
            Email = result.Email,
            Token = result.Token,
        };

        return Ok(response);
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authenticationService.Register(request.Handle, request.Name, request.UniqueName, request.Email, request.Password);

        var response = new AuthenticationResponse
        {
            Id = result.Id,
            Handle = result.Handle,
            Name = result.Name,
            UniqueName = result.UniqueName,
            Email = result.Email,
            Token = result.Token,
        };

        return Ok(response);
    }
}
