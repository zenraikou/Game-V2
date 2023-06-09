using ErrorOr;
using Game.Contracts.Player;
using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Common.Errors;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Queries.Get;

public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, ErrorOr<PlayerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpressionMapper _expressionMapper;

    public GetPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper, IExpressionMapper expressionMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expressionMapper = expressionMapper;
    }

    public async Task<ErrorOr<PlayerResponse>> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var expression = _expressionMapper.MapExpression<PlayerRequest, Player>(request.Expression);
        var player = await _unitOfWork.Players.Get(expression);

        if (player == null)
        {
            return Errors.Player.NotFound;
        }

        var response = _mapper.Map<PlayerResponse>(player);
        return response;
    }
}
