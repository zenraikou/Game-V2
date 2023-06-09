using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Game.Core.Services.Sessions.Commands;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class PatchSessionHandler : IRequestHandler<PatchSessionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatchSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PatchSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.Id == request.Id);

        if (session == null)
        {
            throw new NotFoundException("Session not found.");
        }

        var sessionRequest = _mapper.Map<SessionRequest>(session);
        request.JsonPatchDocument.ApplyTo(sessionRequest);
        _mapper.Map(sessionRequest, session);

        await _unitOfWork.Sessions.Update(session);
        await _unitOfWork.Save();

        return await Unit.Task;
    }
}
