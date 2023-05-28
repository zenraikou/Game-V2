using System.Linq.Expressions;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Get;

public class GetSessionHandler : IRequestHandler<GetSessionQuery, SessionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponse> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var expression = _mapper.Map<Expression<Func<Session, bool>>>(request.Expression);
        var session = await _unitOfWork.Sessions.Get(expression);

        if (session is null)
        {
            throw new NotFoundException("Session not found.");
        }

        var response = _mapper.Map<SessionResponse>(session);
        return response;
    }
}
