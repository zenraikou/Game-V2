using System.Linq.Expressions;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Get;

public class GetSessionHandler : IRequestHandler<GetSessionQuery, Session?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Session?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Sessions.Get(request.Expression);
    }
}
