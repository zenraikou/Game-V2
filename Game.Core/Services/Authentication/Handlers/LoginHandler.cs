using ErrorOr;
using Game.Contracts.Authentication;
using Game.Contracts.Session;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Players.Queries;
using Game.Core.Services.Sessions.Commands;
using Game.Core.Services.Sessions.Queries;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Game.Core.Services.Authentication.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LoginHandler(ILogger<LoginHandler> logger, ISender mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var playerResponse = await _mediator.Send(new GetPlayerQuery(p => p.UniqueName == request.Login.UniqueName));

        if (playerResponse.IsError || !BCrypt.Net.BCrypt.Verify(request.Login.Password, playerResponse.Value.PasswordHash))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var fingerprint = await _mediator.Send(new GetFingerprintQuery());

        var sessionResponse = await _mediator.Send(new GetSessionQuery(s => s.Fingerprint == fingerprint.Value));

        if (!sessionResponse.IsError)
        {
            await _mediator.Send(new DeleteSessionCommand(sessionResponse.Value.Id));
        }

        var jwt = await _mediator.Send(new GenerateJWTCommand(playerResponse.Value.Id.ToString(), playerResponse.Value.Role));

        sessionResponse = await _mediator.Send(new GenerateSessionCommand(jwt));
        var sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);

        await _mediator.Send(new PostSessionCommand(sessionRequest));

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
