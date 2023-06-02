using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Players.Get;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Update;

public class UpdatePlayerHandler : IRequestHandler<UpdatePlayerCommand, PlayerResponse>
{
    private readonly ISender _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlayerHandler(ISender mediator, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlayerResponse> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var getPlayerQuery = new GetPlayerQuery(p => p.UniqueName == request.Player.UniqueName);
        var playerResponse = await _mediator.Send(getPlayerQuery);
        playerResponse = _mapper.Map<PlayerResponse>(request.Player);
        var player = _mapper.Map<Player>(playerResponse);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();
        var response = _mapper.Map<PlayerResponse>(player);
        return response;
    }
}
