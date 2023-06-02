using Game.API.Attributes;
using Game.Contracts.Player;
using Game.Core.Services.Players.Delete;
using Game.Core.Services.Players.Get;
using Game.Core.Services.Players.GetAll;
using Game.Core.Services.Players.Post;
using Game.Core.Services.Players.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Authorize(Roles = "Admin")]
[Fingerprinting]
[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ISender _mediator;

    public PlayerController(ILogger logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("~/api/[controller]s")] /* GET: {host}/api/players */
    public async Task<ActionResult<IEnumerable<PlayerResponse>>> GetAll()
    {
        var getAllPlayersQuery = new GetAllPlayersQuery();
        var response = await _mediator.Send(getAllPlayersQuery);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}")] /* GET: {host}/api/player/{id} */
    public async Task<ActionResult<PlayerResponse>> Get(Guid id)
    {
        var getPlayerQuery = new GetPlayerQuery(p => p.Id == id);
        var response = await _mediator.Send(getPlayerQuery);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost] /* POST: {host}/api/player */
    public async Task<ActionResult<PlayerResponse>> Post(PlayerRequest request)
    {
        var postPlayerCommand = new PostPlayerCommand(request);
        var response = await _mediator.Send(postPlayerCommand);
        return CreatedAtAction(nameof(Get), new { Id = response.Id }, response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPut] /* PUT: {host}/api/player */
    public async Task<ActionResult<PlayerResponse>> Put(PlayerRequest request)
    {
        var updatePlayerCommand = new UpdatePlayerCommand(request);
        var response = await _mediator.Send(updatePlayerCommand);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [HttpDelete] /* DELETE: {host}/api/delete/player */
    public async Task<ActionResult<IActionResult>> Delete(PlayerRequest request)
    {
        var deletePlayerCommand = new DeletePlayerCommand(request);
        await _mediator.Send(deletePlayerCommand);
        return NoContent();
    }
}
