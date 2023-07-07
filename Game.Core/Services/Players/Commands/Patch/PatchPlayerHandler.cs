using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using Mapster;
using MediatR;

namespace Game.Core.Services.Players.Commands.Patch;

public class PatchPlayerHandler : IRequestHandler<PatchPlayerCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatchPlayerHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(PatchPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = request.Player.Adapt<Player>();

        player.Id = request.Id;

        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return Result.Updated;
    }
}
