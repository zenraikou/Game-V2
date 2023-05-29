using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Generator.GenerateJWT;
using Game.Core.Services.Generator.GenerateSession;
using Game.Core.Services.Players.Get;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RegisterHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Register.Player.UniqueName);
        var playerResponse = await _mediator.Send(getPlayerQuery);

        if (playerResponse is not null)
        {
            throw new BadRequestException("ID is not available.");
        }

        request.Register.Player.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Register.Password);
        playerResponse = _mapper.Map<PlayerResponse>(request.Register.Player); // GUID here is empty or all zero

        var player = _mapper.Map<Player>(playerResponse); // GUID here is not empty
        await _unitOfWork.Players.Post(player);
        await _unitOfWork.Save();

        var generateJWTRequest = _mapper.Map<GenerateJWTRequest>(player);
        var generateJWTCommand = new GenerateJWTCommand(generateJWTRequest);
        var generateJWTResponse = await _mediator.Send(generateJWTCommand);

        var generateSessionRequest = _mapper.Map<GenerateSessionRequest>(generateJWTResponse);
        var generateSessionCommand = new GenerateSessionCommand(generateSessionRequest);
        var sessionResponse = await _mediator.Send(generateSessionCommand);

        var session = _mapper.Map<Session>(sessionResponse);
        await _unitOfWork.Sessions.Post(session);
        await _unitOfWork.Save();

        var response = new AuthenticationResponse { JWT = generateJWTResponse.JWT };
        return response;
    }
}
