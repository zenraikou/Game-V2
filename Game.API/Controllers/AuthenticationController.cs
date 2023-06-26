using Game.API.Attributes;
using Game.Contracts.Authentication;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting]
[Route("api/auth")]
public class AuthenticationController : APIController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommand = new RegisterCommand(request);
        var response = await _mediator.Send(registerCommand);

        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
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

    [HttpPost("refresh-token"), Fingerprinting]
    public async Task<ActionResult<AuthenticationResponse>> RefreshToken()
    {
        var refreshTokenCommand = new RefreshTokenCommand();
        var response = await _mediator.Send(refreshTokenCommand);
        return Ok(response);
    }
}
