using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Update;

public class UpdatePlayerHandler : IRequestHandler<UpdatePlayerCommand, PlayerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlayerResponse> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = _mapper.Map<Player>(request.Player);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();
        var response = _mapper.Map<PlayerResponse>(player);
        return response;
    }
}
