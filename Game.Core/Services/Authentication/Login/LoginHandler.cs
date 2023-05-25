using Game.Contracts.Authentication;
using Game.Core.Services.Authentication.Login;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public class LoginHandler : IRequestHandler<LoginCommand, LoginRequest>
{
    public Task<LoginRequest> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
