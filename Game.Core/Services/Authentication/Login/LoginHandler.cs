using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Session;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Login;
using Game.Core.Services.Claims;
using Game.Core.Services.Generator.GenerateJWT;
using Game.Core.Services.Generator.GenerateSession;
using Game.Core.Services.Header;
using Game.Core.Services.Players.Get;
using Game.Core.Services.Sessions.Get;
using Game.Core.Services.Sessions.Post;
using Game.Core.Services.Sessions.Update;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Game.Core.Services.Authentication.Register;

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

        var generateJWTRequest = _mapper.Map<GenerateJWTRequest>(playerResponse);
        var generateJWTCommand = new GenerateJWTCommand(generateJWTRequest);
        var generateJWTResponse = await _mediator.Send(generateJWTCommand);

        var generateSessionRequest = _mapper.Map<GenerateSessionRequest>(generateJWTResponse);
        var generateSessionCommand = new GenerateSessionCommand(generateSessionRequest);
        var generateSessionResponse = await _mediator.Send(generateSessionCommand);

        var getClaimQuery = new GetClaimQuery(c => c.Type == "jti");
        var jti = await _mediator.Send(getClaimQuery);

        var getHeaderQuery = new GetHeaderQuery("Fingerprint");
        var fingerprint = await _mediator.Send(getHeaderQuery);

        var getSessionQuery = new GetSessionQuery(s => s.JTI == jti);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        var sessionRequest = _mapper.Map<SessionRequest>(generateSessionResponse);

        if (sessionResponse is null || !sessionResponse.Fingerprint.Equals(fingerprint))
        {
            var postSessionCommand = new PostSessionCommand(sessionRequest);
            await _mediator.Send(postSessionCommand);
        }
        else
        {
            var updateSessionCommand = new UpdateSessionCommand(sessionRequest);
            await _mediator.Send(updateSessionCommand);
        }

        var response = new AuthenticationResponse { JWT = generateJWTResponse.JWT };
        return response;
    }
}
