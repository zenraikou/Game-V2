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
    public ActionResult<AuthenticationResponse> Login(LoginRequest request)
    {
        var response = _authenticationService.Login(request);
        return Ok(response);
    }

    [HttpPost("register")]
    public ActionResult<AuthenticationResponse> Register(RegisterRequest request)
    {
        var response = _authenticationService.Register(request);
        return Ok(response);
    }
}
