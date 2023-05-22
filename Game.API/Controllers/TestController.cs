using System.IdentityModel.Tokens.Jwt;
using Game.API.Attributes;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Authorize]
[Fingerprinting]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ISessionRepository _sessionRepository;

    public TestController(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
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
        var jwt = Request.Headers["Authorization"].ToString().Split(' ')[1];
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == "jti")!.Value;
        var session = await _sessionRepository.Get(s => s.JTI == jti);

        if (session is null)
        {
            return NotFound("Fingerprint not found.");
        }

        return Ok(session);
    }
}
