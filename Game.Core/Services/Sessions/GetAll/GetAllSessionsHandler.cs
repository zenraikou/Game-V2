using Game.Contracts.Session;
using Game.Core.Common.Interfaces.ExpressionMapper;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.GetAll;

public class GetAllSessionsHandler : IRequestHandler<GetAllSessionsQuery, IEnumerable<SessionResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpressionMapper _expressionMapper;

    public GetAllSessionsHandler(IUnitOfWork unitOfWork, IMapper mapper, IExpressionMapper expressionMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expressionMapper = expressionMapper;
    }

    public async Task<IEnumerable<SessionResponse>> Handle(GetAllSessionsQuery request, CancellationToken cancellationToken)
    {
        var expression = _expressionMapper.MapExpression<SessionRequest, Session>(request.Expression!);
        var sessions = await _unitOfWork.Sessions.GetAll(expression);
        var response = _mapper.Map<IEnumerable<SessionResponse>>(sessions);
        return response;
    }
}
