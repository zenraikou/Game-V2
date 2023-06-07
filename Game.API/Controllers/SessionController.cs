using Game.Contracts.Session;
using Game.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

public class SessionController : ControllerBase
{
    private readonly ISender _mediator;

    public SessionController(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task<ActionResult<IEnumerable<SessionResponse>>> GetAll()
    {
        throw new UnimplementedException("Not implemented.");
    }
}
