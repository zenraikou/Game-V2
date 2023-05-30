using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Generator.GenerateSession;
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
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Register.UniqueName);
        var player = await _mediator.Send(getPlayerQuery);

        if (player is not null)
        {
            throw new BadRequestException("ID is not available.");
        }

        request.Register.Password = BCrypt.Net.BCrypt.HashPassword(request.Register.Password);
        player = _mapper.Map<Player>(request.Register);

        await _unitOfWork.Players.Post(player);
        await _unitOfWork.Save();

        // var generateJWTRequest = _mapper.Map<GenerateJWTRequest>(player);
        var generateJWTCommand = new GenerateJWTCommand(player);
        var jwt = await _mediator.Send(generateJWTCommand);

        // var generateSessionRequest = _mapper.Map<GenerateSessionRequest>(generateJWTResponse);
        var generateSessionCommand = new GenerateSessionCommand(jwt);
        var session = await _mediator.Send(generateSessionCommand);

        // var session = _mapper.Map<Session>(sessionResponse);
        await _unitOfWork.Sessions.Post(session);
        await _unitOfWork.Save();

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
