using MediatR;

namespace Game.Core.Services.Authentication.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    public Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
