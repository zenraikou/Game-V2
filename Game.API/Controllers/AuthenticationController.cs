using Game.API.Attributes;
using Game.Contracts.Authentication;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Login;
using Game.Core.Services.Authentication.Logout;
using Game.Core.Services.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting]
[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
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
        var loginCommand = new LoginCommand(request);
        var response = await _mediator.Send(loginCommand);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout"), Authorize, Fingerprinting]
    public async Task<IActionResult> Logout()
    {
        var logoutCommand = new LogoutCommand();
        await _mediator.Send(logoutCommand);
        return Ok();
    }

    [HttpPost("refresh-token")]
    public Task<AuthenticationResponse> RefreshToken()
    {
        throw new UnimplementedException("Not yet implemented.");
    }
}
