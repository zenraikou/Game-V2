using Game.API.Attributes;
using Game.Contracts.Player;
using Game.Core.Services.Players.Commands.Delete;
using Game.Core.Services.Players.Commands.Patch;
using Game.Core.Services.Players.Commands.Post;
using Game.Core.Services.Players.Commands.Put;
using Game.Core.Services.Players.Queries.Get;
using Game.Core.Services.Players.Queries.GetAll;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting, Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class PlayerController : APIController
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
        var response = await _mediator.Send(new GetAllPlayersQuery());

        if (!response.Any())
            return NoContent();

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}")] /* GET: {host}/api/player/{id} */
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new GetPlayerQuery(p => p.Id == id));
        return response.Match(
            response => Ok(response),
            errors => Problem(errors));
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost] /* POST: {host}/api/player */
    public async Task<ActionResult<PlayerResponse>> Post(PlayerRequest request)
    {
        var response = await _mediator.Send(new PostPlayerCommand(request));
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("{id}")] /* PUT: {host}/api/player/{id} */
    public async Task<IActionResult> Put(Guid id, PlayerRequest request)
    {
        await _mediator.Send(new PutPlayerCommand(id, request));
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPatch("{id}")] /* PATCH: {host}/api/player/{id} */
    public async Task<IActionResult> Patch(Guid id, JsonPatchDocument<PlayerRequest> jsonPatchDocument)
    {
        var result = await _mediator.Send(new GetPlayerQuery(p => p.Id == id));

        var request = result.Value.Adapt<PlayerRequest>();
        jsonPatchDocument.ApplyTo(request);

        var response = await _mediator.Send(new PatchPlayerCommand(id, request));

        if (response.IsError)
            return Problem(response.Errors);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")] /* DELETE: {host}/api/player/{id} */
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeletePlayerCommand(id));
        return NoContent();
    }
}
