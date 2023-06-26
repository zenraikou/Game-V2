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
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Login.UniqueName);
        var result = await _mediator.Send(getPlayerQuery);

        if (result.IsError)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var playerResponse = result.Value;

        if (!BCrypt.Net.BCrypt.Verify(request.Login.Password, playerResponse.PasswordHash))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var getFingerprintQuery = new GetFingerprintQuery();
        var fingerprint = await _mediator.Send(getFingerprintQuery);

        var getSessionQuery = new GetSessionQuery(s => s.Fingerprint == fingerprint);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        SessionRequest sessionRequest;

        if (sessionResponse != null)
        {
            sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
            var deleteSessionCommand = new DeleteSessionCommand(sessionRequest.Id);
            await _mediator.Send(deleteSessionCommand);
        }

        var generateJWTCommand = new GenerateJWTCommand(playerResponse.Id.ToString(), playerResponse.Role);
        var jwt = await _mediator.Send(generateJWTCommand);

        var generateSessionCommand = new GenerateSessionCommand(jwt);
        sessionResponse = await _mediator.Send(generateSessionCommand);

        sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
        var postSessionCommand = new PostSessionCommand(sessionRequest);
        await _mediator.Send(postSessionCommand);

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
