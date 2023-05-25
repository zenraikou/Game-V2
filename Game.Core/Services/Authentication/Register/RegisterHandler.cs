using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterRequest>
{
    public Task<RegisterRequest> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
