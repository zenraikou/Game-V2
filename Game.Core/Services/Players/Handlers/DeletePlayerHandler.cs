using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Players.Commands;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePlayerHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            throw new NotFoundException("Player not found.");
        }

        await _unitOfWork.Players.Delete(player);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
