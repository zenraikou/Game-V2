using Game.API.Attributes;
using Game.Contracts.Session;
using Game.Core.Services.Sessions.Commands.Delete;
using Game.Core.Services.Sessions.Commands.Patch;
using Game.Core.Services.Sessions.Commands.Post;
using Game.Core.Services.Sessions.Commands.Put;
using Game.Core.Services.Sessions.Queries.Get;
using Game.Core.Services.Sessions.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Fingerprinting, Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class SessionController : APIController
{
    private readonly ISender _mediator;

    public SessionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("~/api/[controller]s")] /* GET: {host}/api/sessions */
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetAll()
    {
        var response = await _mediator.Send(new GetAllSessionsQuery());

        if (!response.Any()) 
            return NoContent();

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}")] /* GET: {host}/api/session/{id} */
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new GetSessionQuery(s => s.Id == id));
        return response.Match(
            response => Ok(response), 
            errors => Problem(errors));
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost] /* POST: {host}/api/session */
    public async Task<ActionResult<SessionResponse>> Post(SessionRequest request)
    {
        var response = await _mediator.Send(new PostSessionCommand(request));
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("{id}")] /* PUT: {host}/api/session/{id} */
    public async Task<IActionResult> Put(Guid id, SessionRequest request)
    {
        await _mediator.Send(new PutSessionCommand(id, request));
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPatch("{id}")] /* PATCH: {host}/api/session/{id} */
    public async Task<IActionResult> Patch(Guid id, JsonPatchDocument<SessionRequest> jsonPatchDocument)
    {
        await _mediator.Send(new PatchSessionCommand(id, jsonPatchDocument));
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")] /* DELETE: {host}/api/session/{id} */
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSessionCommand(id));
        return NoContent();
    }
}
