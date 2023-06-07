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
    private readonly ISender _mediator;

    public PlayerController(ISender mediator)
    {
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("{id}")] /* PUT: {host}/api/player/{id} */
    public async Task<IActionResult> Put(Guid id, PlayerRequest request)
    {
        var updatePlayerCommand = new UpdatePlayerCommand(id, request);
        await _mediator.Send(updatePlayerCommand);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")] /* DELETE: {host}/api/delete/player/{id} */
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletePlayerCommand = new DeletePlayerCommand(id);
        await _mediator.Send(deletePlayerCommand);
        return NoContent();
    }
}
