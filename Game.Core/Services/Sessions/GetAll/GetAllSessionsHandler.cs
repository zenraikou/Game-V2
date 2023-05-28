using System.Linq.Expressions;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.GetAll;

public class GetAllSessionsHandler : IRequestHandler<GetAllSessionsQuery, IEnumerable<SessionResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSessionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SessionResponse>> Handle(GetAllSessionsQuery request, CancellationToken cancellationToken)
    {
        var expression = _mapper.Map<Expression<Func<Session, bool>>>(request.Expression!);
        var sessions = await _unitOfWork.Sessions.GetAll(expression);
        var response = _mapper.Map<IEnumerable<SessionResponse>>(sessions);
        return response;
    }
}
