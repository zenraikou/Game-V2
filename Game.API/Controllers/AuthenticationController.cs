using Game.API.Attributes;
using Game.Contracts.Authentication;
using Game.Core.Services.Authentication.Register;
using Game.Core.TempServices.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting]
[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IMediator mediator, IAuthenticationService authenticationService)
    {
        _mediator = mediator;
        _authenticationService = authenticationService;
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest request)
    {
        var registerCommand = new RegisterCommand(request);
        var response = await _mediator.Send(registerCommand);
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
