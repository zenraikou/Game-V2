using ErrorOr;
using Game.Contracts.Session;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Sessions.Commands;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class PatchSessionHandler : IRequestHandler<PatchSessionCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatchSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Updated>> Handle(PatchSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.Id == request.Id);

        if (session == null)
        {
            return Errors.Session.NotFound;
        }

        var sessionRequest = _mapper.Map<SessionRequest>(session);
        request.JsonPatchDocument.ApplyTo(sessionRequest);
        _mapper.Map(sessionRequest, session);

        await _unitOfWork.Sessions.Update(session);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Updated);
    }
}
