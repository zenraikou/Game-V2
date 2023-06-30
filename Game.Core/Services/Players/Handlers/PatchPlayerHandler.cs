using ErrorOr;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Players.Commands;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class PatchPlayerHandler : IRequestHandler<PatchPlayerCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatchPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Updated>> Handle(PatchPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            return Errors.Player.NotFound;
        }

        var playerRequest = _mapper.Map<PlayerRequest>(player);
        request.JsonPatchDocument.ApplyTo(playerRequest);

        _mapper.Map(playerRequest, player);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Updated);
    }
}
