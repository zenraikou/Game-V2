using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Patch;

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
        var session = _mapper.Map<Session>(request.Session);
        await _unitOfWork.Sessions.Update(session);
        await _unitOfWork.Save();

        return Result.Updated;
    }
}
