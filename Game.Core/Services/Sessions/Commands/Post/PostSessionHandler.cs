using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Post;

public class PostSessionHandler : IRequestHandler<PostSessionCommand, SessionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PostSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponse> Handle(PostSessionCommand request, CancellationToken cancellationToken)
    {
        var session = _mapper.Map<Session>(request.Session);
        await _unitOfWork.Sessions.Post(session);
        await _unitOfWork.Save();

        var response = _mapper.Map<SessionResponse>(session);
        return response;
    }
}
