using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Players.Commands;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Handlers;

public class PutPlayerHandler : IRequestHandler<PutPlayerCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PutPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Updated>> Handle(PutPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.Get(p => p.Id == request.Id);

        if (player == null)
        {
            return Errors.Player.NotFound;
        }

        _mapper.Map(request.Player, player);
        await _unitOfWork.Players.Update(player);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Updated);
    }
}
