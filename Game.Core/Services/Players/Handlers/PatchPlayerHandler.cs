using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Players.Commands;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Game.Core.Services.Players.Handlers;

public class PatchPlayerHandler : IRequestHandler<PatchPlayerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatchPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PatchPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            throw new NotFoundException("Player not found.");
        }

        var playerRequest = _mapper.Map<PlayerRequest>(player);
        request.JsonPatchDocument.ApplyTo(playerRequest);
        _mapper.Map(playerRequest, player);

        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
