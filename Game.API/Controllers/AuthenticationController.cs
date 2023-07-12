using Game.API.Attributes;
using Game.Contracts.Authentication;
using Game.Core.Services.Authentications.Commands.Login;
using Game.Core.Services.Authentications.Commands.Logout;
using Game.Core.Services.Authentications.Commands.RefreshToken;
using Game.Core.Services.Authentications.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting, Authorize]
[Route("api/auth")]
public class AuthenticationController : APIController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("register"), AllowAnonymous] /* GET: {host}/api/auth/register */
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _mediator.Send(new RegisterCommand(request));

        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login"), AllowAnonymous] /* GET: {host}/api/auth/login */
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _mediator.Send(new LoginCommand(request));

        return response.Match(
            response => Ok(response), 
            errors => Problem(errors));
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout")] /* GET: {host}/api/auth/logout */
    public async Task<IActionResult> Logout()
    {
        var response = await _mediator.Send(new LogoutCommand());

        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("refresh-token"), AllowAnonymous] /* GET: {host}/api/auth/refresh-token */
    public async Task<IActionResult> RefreshToken()
    {
        var response = await _mediator.Send(new RefreshTokenCommand());

        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
    }
}
