using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Session;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Login;
using Game.Core.Services.Generator.GenerateJWT;
using Game.Core.Services.Generator.GenerateSession;
using Game.Core.Services.Players.Get;
using Game.Core.Services.Sessions.Post;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public LoginHandler(ISender mediator, IMapper mapper)
    {
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

        var sessionRequest = _mapper.Map<SessionRequest>(generateSessionResponse);
        var postSessionCommand = new PostSessionCommand(sessionRequest);
        await _mediator.Send(postSessionCommand);

        var response = new AuthenticationResponse { JWT = generateJWTResponse.JWT };
        return response;
    }
}
