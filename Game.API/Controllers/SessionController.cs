using Game.API.Attributes;
using Game.Contracts.Session;
using Game.Core.Services.Sessions.Commands;
using Game.Core.Services.Sessions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[Authorize(Roles = "Admin")]
[Fingerprinting]
[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly ILogger<SessionController> _logger;
    private readonly ISender _mediator;

    public SessionController(ILogger<SessionController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("~/api/[controller]s")] /* GET: {host}/api/sessions */
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetAll()
    {
        var getAllSessionsQuery = new GetAllSessionsQuery();
        var response = await _mediator.Send(getAllSessionsQuery);

        if (!response.Any())
        {
            _logger.LogInformation("204 No Content");
            return NoContent();
        }

        _logger.LogInformation("200 OK");
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}")] /* GET: {host}/api/session/{id} */
    public async Task<ActionResult<SessionResponse>> Get(Guid id)
    {
        var getSessionQuery = new GetSessionQuery(s => s.Id == id);
        var response = await _mediator.Send(getSessionQuery);

        _logger.LogInformation("200 OK");
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost] /* POST: {host}/api/session */
    public async Task<ActionResult<SessionResponse>> Post(SessionRequest request)
    {
        var postSessionCommand = new PostSessionCommand(request);
        var response = await _mediator.Send(postSessionCommand);
        
        _logger.LogInformation("201 Created");
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    [HttpPut("{id}")] /* PUT: {host}/api/session/{id} */
    public async Task<IActionResult> Put(Guid id, SessionRequest request)
    {
        var updateSessionCommand = new UpdateSessionCommand(id, request);
        await _mediator.Send(updateSessionCommand);

        _logger.LogInformation("204 No Content");
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")] /* DELETE: {host}/api/session/{id} */
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteSessionCommand = new DeleteSessionCommand(id);
        await _mediator.Send(deleteSessionCommand);

        _logger.LogInformation("204 No Content");
        return NoContent();
    }
}
