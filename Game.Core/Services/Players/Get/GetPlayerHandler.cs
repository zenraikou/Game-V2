using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Players.Get;

public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, Player?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Player?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Players.Get(request.Expression);
    }
}
