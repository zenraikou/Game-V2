using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Sessions.Commands;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class UpdateSessionHandler : IRequestHandler<UpdateSessionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.Id == request.Id);

        if (session == null)
        {
            throw new NotFoundException("Session not found.");
        }

        _mapper.Map(request.Session, session);

        await _unitOfWork.Sessions.Update(session);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
