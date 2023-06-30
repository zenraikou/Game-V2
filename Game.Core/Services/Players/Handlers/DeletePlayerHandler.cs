using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Players.Commands;
using Game.Domain.Common.Errors;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, ErrorOr<Deleted>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePlayerHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            return Errors.Player.NotFound;
        }

        await _unitOfWork.Players.Delete(player);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Deleted);
    }
}
