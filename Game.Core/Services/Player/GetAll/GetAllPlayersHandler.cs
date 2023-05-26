using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Player.GetAll;

public class GetAllPlayersHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<PlayerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlayerResponse>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = await _unitOfWork.Players.GetAll();
        return _mapper.Map<IEnumerable<PlayerResponse>>(players);
    }
}
