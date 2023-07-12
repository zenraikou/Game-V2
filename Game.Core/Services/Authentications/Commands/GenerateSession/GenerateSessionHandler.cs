using ErrorOr;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Settings;
using Game.Core.Services.Authentications.Queries.GetFingerprint;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.Extensions.Options;

namespace Game.Core.Services.Authentications.Commands.GenerateSession;

public class GenerateSessionHandler : IRequestHandler<GenerateSessionCommand, ErrorOr<SessionResponse>>
{
    private readonly ISender _mediator;
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;

    public GenerateSessionHandler(ISender mediator, ITime time, IOptions<JWTSettings> jwtSettings)
    {
        _mediator = mediator;
        _time = time;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ErrorOr<SessionResponse>> Handle(GenerateSessionCommand request, CancellationToken cancellationToken)
    {
        var fingerprint = await _mediator.Send(new GetFingerprintQuery(), cancellationToken);

        if (fingerprint.IsError)
        {
            return Errors.Authorization.Unauthorized;
        }

        var response = new SessionResponse
        {
            Id = Guid.NewGuid(),
            Fingerprint = fingerprint.Value,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        return response;
    }
}
