using ErrorOr;
using Game.Contracts.Session;
using Game.Core.Common.Constants;
using Game.Core.Services.Authentications.Queries.GetClaim;
using Game.Core.Services.Authentications.Queries.GetFingerprint;
using Game.Core.Services.Sessions.Commands.Delete;
using Game.Core.Services.Sessions.Queries.Get;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, ErrorOr<Success>>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LogoutHandler(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellatioSnToken)
    {
        var fingerprint = await _mediator.Send(new GetFingerprintQuery());
        var jti = await _mediator.Send(new GetClaimQuery(c => c.Type == JWTClaims.JTI));

        if (fingerprint.IsError || jti.IsError)
        {
            return Errors.Authorization.Unauthorized;
        }

        var sessionResponse = await _mediator.Send(new GetSessionQuery(s => s.Id == Guid.Parse(jti.Value)));

        if (sessionResponse.Value.Fingerprint != fingerprint.Value)
        {
            return Errors.Authorization.Unauthorized;
        }

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        await _mediator.Send(new DeleteSessionCommand(sessionRequest.Id));

        return Result.Success;
    }
}
