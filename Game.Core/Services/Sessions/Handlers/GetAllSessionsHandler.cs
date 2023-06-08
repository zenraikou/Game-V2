using System.Linq.Expressions;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Sessions.Queries;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

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
        Expression<Func<Session, bool>>? expression = null;

        if (request.Expression != null)
        {
            expression = _expressionMapper.MapExpression<SessionRequest, Session>(request.Expression);
        }

        var sessions = await _unitOfWork.Sessions.GetAll(expression);

        var response = _mapper.Map<IEnumerable<SessionResponse>>(sessions);
        return response;
    }
}
