using Game.Contracts.Session;
using Game.Core.Exceptions;
using Game.Core.Services.Claims;
using Game.Core.Services.Sessions.Delete;
using Game.Core.Services.Sessions.Get;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentication.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LogoutHandler(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var getClaimCommand = new GetClaimCommand(c => c.Type == "jti");
        var claim = await _mediator.Send(getClaimCommand);

        var getSessionQuery = new GetSessionQuery(s => s.JTI == claim);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        if (sessionResponse is null)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        var deleteSessionCommand = new DeleteSessionCommand(sessionRequest);
        await _mediator.Send(deleteSessionCommand);

        return await Unit.Task;
    }
}
