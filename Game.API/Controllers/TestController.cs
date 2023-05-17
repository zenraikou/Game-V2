using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

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

    [HttpGet("greetings"), Authorize(Roles = "Admin, User")]
    public ActionResult<string> Greetings()
    {
        var greetings = "Hello, World!";
        return Ok(greetings);
    }

    [HttpGet("user-refresh-token"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> UserRefreshToken()
    {
        var id = _userService.GetUserClaimsId();
        var user = await _userRepository.Get(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User does not exist.");
        }

        var refreshToken = await _refreshTokenRepository.Get(user.Id);
        return Ok(refreshToken);
    }
}
