using ErrorOr;
using Game.Contracts.Authentication;
using Game.Contracts.Session;
using Game.Core.Services.Authentications.Commands.GenerateJWT;
using Game.Core.Services.Authentications.Commands.GenerateSession;
using Game.Core.Services.Authentications.Queries.GetFingerprint;
using Game.Core.Services.Players.Queries.Get;
using Game.Core.Services.Sessions.Commands.Delete;
using Game.Core.Services.Sessions.Commands.Post;
using Game.Core.Services.Sessions.Queries.Get;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LoginHandler(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var playerResponse = await _mediator.Send(new GetPlayerQuery(p => p.UniqueName == request.Login.UniqueName), cancellationToken);

        if (playerResponse.IsError || !Cypher.Verify(request.Login.Password, playerResponse.Value.PasswordHash))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var fingerprint = await _mediator.Send(new GetFingerprintQuery(), cancellationToken);
        var sessionResponse = await _mediator.Send(new GetSessionQuery(s => s.Fingerprint == fingerprint.Value), cancellationToken);

        if (!sessionResponse.IsError)
        {
            await _mediator.Send(new DeleteSessionCommand(sessionResponse.Value.Id), cancellationToken);
        }

        sessionResponse = await _mediator.Send(new GenerateSessionCommand(), cancellationToken);

        if (sessionResponse.IsError)
        {
            return Errors.Authorization.Unauthorized;
        }

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse.Value);
        await _mediator.Send(new PostSessionCommand(sessionRequest), cancellationToken);

        var jwt = await _mediator.Send(new GenerateJWTCommand(playerResponse.Value.Id.ToString(), playerResponse.Value.Role, sessionRequest.Id.ToString()), cancellationToken);

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
