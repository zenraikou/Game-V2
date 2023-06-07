using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Session;
using Game.Core.Common.Constants;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Players.Queries;
using Game.Core.Services.Sessions.Commands;
using Game.Core.Services.Sessions.Queries;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Game.Core.Services.Authentication.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private ILogger<LoginHandler> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LoginHandler(ILogger<LoginHandler> logger, ISender mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Login.UniqueName);
        var playerResponse = await _mediator.Send(getPlayerQuery);

        if (playerResponse is null || !BCrypt.Net.BCrypt.Verify(request.Login.Password, playerResponse.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }
        
        var getHeaderQuery = new GetHeaderQuery(HTTPHeaders.Fingerprint);
        var fingerprint = await _mediator.Send(getHeaderQuery);

        var getSessionQuery = new GetSessionQuery(s => s.Fingerprint == fingerprint);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        SessionRequest sessionRequest;

        if (sessionResponse is not null)
        {
            sessionRequest = _mapper.Map<SessionRequest>(sessionResponse);
            var deleteSessionCommand = new DeleteSessionCommand(sessionRequest.Id);
            await _mediator.Send(deleteSessionCommand);
        }

        var generateJWTRequest = _mapper.Map<GenerateJWTRequest>(playerResponse);
        var generateJWTCommand = new GenerateJWTCommand(generateJWTRequest);
        var generateJWTResponse = await _mediator.Send(generateJWTCommand);

        var generateSessionRequest = _mapper.Map<GenerateSessionRequest>(generateJWTResponse);
        var generateSessionCommand = new GenerateSessionCommand(generateSessionRequest);
        var generateSessionResponse = await _mediator.Send(generateSessionCommand);

        sessionRequest = _mapper.Map<SessionRequest>(generateSessionResponse);
        var postSessionCommand = new PostSessionCommand(sessionRequest);
        await _mediator.Send(postSessionCommand);

        var response = new AuthenticationResponse { JWT = generateJWTResponse.JWT };
        return response;
    }
}
