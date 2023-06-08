using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Players.Commands;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class UpdatePlayerHandler : IRequestHandler<UpdatePlayerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            throw new NotFoundException("Player not found.");
        }

        _mapper.Map(request.Player, player);

        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
