using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("greetings"), Authorize(Roles = "Admin, User")]
    public ActionResult<string> Greetings()
    {
        var greetings = "Hello, World!";
        return Ok(greetings);
    }
}
