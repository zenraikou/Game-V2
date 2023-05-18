using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TestController(IUserService userService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    [HttpGet("greetings")]
    public ActionResult<string> Greetings()
    {
        var greetings = "Hello, World!";
        return Ok(greetings);
    }
}
