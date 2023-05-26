using System.Linq.Expressions;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Get;

public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, PlayerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlayerResponse> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var expression = _mapper.Map<Expression<Func<Player, bool>>>(request.Expression);
        var player = await _unitOfWork.Players.Get(expression);

        if (player is null)
        {
            throw new NotFoundException("Player not found.");
        }

        var response = _mapper.Map<PlayerResponse>(player);
        return response;
    }
}
