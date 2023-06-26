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
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Register.UniqueName);
        var result = await _mediator.Send(getPlayerQuery);

        if (!result.IsError)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var playerRequest = _mapper.Map<PlayerRequest>(request.Register);
        var postPlayerCommand = new PostPlayerCommand(playerRequest);
        var playerResponse = await _mediator.Send(postPlayerCommand);

        var generateJWTCommand = new GenerateJWTCommand(playerResponse.Id.ToString(), playerResponse.Role);
        var jwt = await _mediator.Send(generateJWTCommand);

        var generateSessionCommand = new GenerateSessionCommand(jwt);
        var sessionResponse = await _mediator.Send(generateSessionCommand);

        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        var postSessionCommand = new PostSessionCommand(sessionRequest);
        await _mediator.Send(postSessionCommand);

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
