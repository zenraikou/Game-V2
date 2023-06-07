using Game.API.Attributes;
using Game.Core.Common.Constants;
using Game.Core.Services.Sessions.Queries;
using Game.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Game.API.Controllers;

[Authorize]
[Fingerprinting]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("greetings")]
    public ActionResult<string> Greetings()
    {
        var greetings = "Hello, World!";
        return Ok(greetings);
    }

    [HttpGet("session")]
    public async Task<ActionResult<Session>> Session()
    {
        var jwt = Request.Headers[HTTPHeaders.Authorization].ToString().Split(' ')[1];
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == JWTClaims.JTI)!.Value;

        var getSessionQuery = new GetSessionQuery(s => s.Id == Guid.Parse(jti));
        var session = await _mediator.Send(getSessionQuery);

        if (session is null)
        {
            return NotFound("Fingerprint not found.");
        }

        return Ok(session);
    }
}
