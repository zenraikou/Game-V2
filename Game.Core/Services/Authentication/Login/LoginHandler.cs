using Game.Contracts.Authentication;
using Game.Core.Services.Authentication.Login;
using MapsterMapper;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LoginHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
