using ErrorOr;
using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Register;

public record RegisterCommand(RegisterRequest Register) : IRequest<ErrorOr<AuthenticationResponse>>;
