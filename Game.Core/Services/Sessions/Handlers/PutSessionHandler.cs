using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Sessions.Commands;
using Game.Domain.Common.Errors;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class PutSessionHandler : IRequestHandler<PutSessionCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PutSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Updated>> Handle(PutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.Id == request.Id);

        if (session == null)
        {
            return Errors.Session.NotFound;
        }

        _mapper.Map(request.Session, session);
        await _unitOfWork.Sessions.Update(session);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Updated);
    }
}
