using ErrorOr;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Services.Sessions.Commands;
using Game.Domain.Common.Errors;
using MediatR;

namespace Game.Core.Services.Sessions.Handlers;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, ErrorOr<Deleted>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSessionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions.Get(s => s.Id == request.Id);

        if (session == null)
        {
            return Errors.Session.NotFound;
        }

        await _unitOfWork.Sessions.Delete(session);
        await _unitOfWork.Save();

        return await Task.FromResult(Result.Deleted);
    }
}
