using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, SessionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponse> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var session = _mapper.Map<Session>(request.Session);
        await _unitOfWork.Sessions.Delete(session);
        await _unitOfWork.Save();
        var response = _mapper.Map<SessionResponse>(session);
        return response;
    }
}
