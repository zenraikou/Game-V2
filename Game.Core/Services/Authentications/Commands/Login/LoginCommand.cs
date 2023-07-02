using ErrorOr;
using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Login;

public record LoginCommand(LoginRequest Login) : IRequest<ErrorOr<AuthenticationResponse>>;
