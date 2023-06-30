using Game.API.Attributes;
using Game.Core.Common.Constants;
using Game.Core.Services.Sessions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Game.API.Controllers;

[Authorize]
[Fingerprinting]
[Route("api/[controller]")]
public class TestController : APIController
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
    public async Task<IActionResult> Session()
    {
        var jwt = Request.Headers[HTTPHeaders.Authorization].ToString().Split(' ')[1];
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == JWTClaims.JTI)!.Value;

        var response = await _mediator.Send(new GetSessionQuery(s => s.Id == Guid.Parse(jti)));

        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
    }
}
