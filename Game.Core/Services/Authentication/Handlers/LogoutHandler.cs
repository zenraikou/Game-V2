using Game.Contracts.Session;
using Game.Core.Common.Constants;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Sessions.Commands;
using Game.Core.Services.Sessions.Queries;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentication.Handlers;

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LogoutHandler(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellatioSnToken)
    {
        var getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.JTI);
        var jti = await _mediator.Send(getClaimQuery);

        var getHeaderQuery = new GetHeaderQuery(HTTPHeaders.Fingerprint);
        var fingerprint = await _mediator.Send(getHeaderQuery);

        var getSessionQuery = new GetSessionQuery(s => s.Id == Guid.Parse(jti));
        var sessionResponse = await _mediator.Send(getSessionQuery);

        if (sessionResponse is null || sessionResponse.Fingerprint != fingerprint)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        var deleteSessionCommand = new DeleteSessionCommand(sessionRequest.Id);
        await _mediator.Send(deleteSessionCommand);

        return await Unit.Task;
    }
}
