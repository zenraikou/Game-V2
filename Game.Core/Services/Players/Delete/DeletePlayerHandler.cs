using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Delete;

public class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, PlayerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlayerResponse> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = _mapper.Map<Player>(request.Player);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();
        var response = _mapper.Map<PlayerResponse>(player);
        return response;
    }
}
