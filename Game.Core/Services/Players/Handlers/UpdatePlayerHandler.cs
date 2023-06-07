using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Players.Commands;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class UpdatePlayerHandler : IRequestHandler<UpdatePlayerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlayerHandler(ISender mediator, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player is null)
        {
            throw new NotFoundException("Player not found.");
        }

        player = _mapper.Map<Player>(request.Player);

        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
