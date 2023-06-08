using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Sessions.Queries;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class GetSessionHandler : IRequestHandler<GetSessionQuery, SessionResponse?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpressionMapper _expressionMapper;

    public GetSessionHandler(IUnitOfWork unitOfWork, IMapper mapper, IExpressionMapper expressionMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expressionMapper = expressionMapper;
    }

    public async Task<SessionResponse?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var expression = _expressionMapper.MapExpression<SessionRequest, Session>(request.Expression);
        var session = await _unitOfWork.Sessions.Get(expression);

        if (session == null)
        {
            throw new NotFoundException("Session not found.");
        }

        var response = _mapper.Map<SessionResponse>(session);
        return response;
    }
}
