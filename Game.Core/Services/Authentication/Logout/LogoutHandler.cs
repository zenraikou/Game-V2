using Game.Contracts.Session;
using Game.Core.Common.Headers;
using Game.Core.Common.JWT;
using Game.Core.Exceptions;
using Game.Core.Services.Claims;
using Game.Core.Services.Header;
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

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellatioSnToken)
    {
        var getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.JTI);
        var jti = await _mediator.Send(getClaimQuery);

        var getHeaderQuery = new GetHeaderQuery(Headers.Fingerprint);
        var fingerprint = await _mediator.Send(getHeaderQuery);

        var getSessionQuery = new GetSessionQuery(s => s.JTI == jti);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        if (sessionResponse is null || sessionResponse.Fingerprint != fingerprint)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        var deleteSessionCommand = new DeleteSessionCommand(sessionRequest.JTI);
        await _mediator.Send(deleteSessionCommand);

        return await Unit.Task;
    }
}
