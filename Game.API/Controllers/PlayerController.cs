using Game.Contracts.Player;
using Game.Core.Services.Players.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

// [Authorize(Roles = "Admin"), Fingerprinting]
[Route("api/[controller]"), ApiController]
public class PlayerController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public PlayerController(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("~/api/[controller]s")]
    public async Task<ActionResult<IEnumerable<PlayerResponse>>> GetAll()
    {
        var query = new GetAllPlayersQuery();
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
