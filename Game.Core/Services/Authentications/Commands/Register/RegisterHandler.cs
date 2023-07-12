using ErrorOr;
using Game.Contracts.Authentication;
using Game.Contracts.Player;
using Game.Contracts.Session;
using Game.Core.Services.Authentications.Commands.GenerateJWT;
using Game.Core.Services.Authentications.Commands.GenerateSession;
using Game.Core.Services.Players.Commands.Post;
using Game.Core.Services.Players.Queries.Get;
using Game.Core.Services.Sessions.Commands.Post;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public RegisterHandler(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var playerResponse = await _mediator.Send(new GetPlayerQuery(p => p.UniqueName == request.Register.UniqueName), cancellationToken);

        if (!playerResponse.IsError)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var playerRequest = _mapper.Map<PlayerRequest>(request.Register);
        playerResponse = await _mediator.Send(new PostPlayerCommand(playerRequest), cancellationToken);

        var sessionResponse = await _mediator.Send(new GenerateSessionCommand(), cancellationToken);

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
