using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Commands.Patch;

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
        var player = _mapper.Map<Player>(request.Player);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return Result.Updated;
    }
}
