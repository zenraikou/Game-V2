using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.JTI == request.Id);

        if (session is null)
        {
            throw new NotFoundException("Session not found.");
        }

        await _unitOfWork.Sessions.Delete(session);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
