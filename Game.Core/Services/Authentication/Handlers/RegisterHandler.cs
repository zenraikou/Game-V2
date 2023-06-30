using ErrorOr;
using Game.Contracts.Authentication;
using Game.Contracts.Player;
using Game.Contracts.Session;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Players.Commands;
using Game.Core.Services.Players.Queries;
using Game.Core.Services.Sessions.Commands;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Mediators.Authentication.Handlers;

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
        var playerResponse = await _mediator.Send(new GetPlayerQuery(p => p.UniqueName == request.Register.UniqueName));

        if (!playerResponse.IsError)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var playerRequest = _mapper.Map<PlayerRequest>(request.Register);
        playerResponse = await _mediator.Send(new PostPlayerCommand(playerRequest));

        var jwt = await _mediator.Send(new GenerateJWTCommand(playerResponse.Value.Id.ToString(), playerResponse.Value.Role));

        var sessionResponse = await _mediator.Send(new GenerateSessionCommand(jwt));
        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);

        await _mediator.Send(new PostSessionCommand(sessionRequest));

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
