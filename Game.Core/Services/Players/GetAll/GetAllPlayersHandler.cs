using System.Linq.Expressions;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.GetAll;

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
        var expression = _mapper.Map<Expression<Func<Player, bool>>>(request.Expression!);
        var players = await _unitOfWork.Players.GetAll(expression);
        var response = _mapper.Map<IEnumerable<PlayerResponse>>(players);
        return response;
    }
}
