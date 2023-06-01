using System.Linq.Expressions;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.ExpressionMapper;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.GetAll;

public class GetAllPlayersHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<PlayerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpressionMapper _expressionMapper;

    public GetAllPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper, IExpressionMapper expressionMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expressionMapper = expressionMapper;
    }

    public async Task<IEnumerable<PlayerResponse>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
    {
        var expression = _expressionMapper.MapExpression<PlayerRequest, Player>(request.Expression!);
        var players = await _unitOfWork.Players.GetAll(expression);
        var response = _mapper.Map<IEnumerable<PlayerResponse>>(players);
        return response;
    }
}
