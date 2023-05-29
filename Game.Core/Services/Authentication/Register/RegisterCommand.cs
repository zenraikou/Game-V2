using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentication.Register;

public record RegisterCommand(RegisterRequest Register) : IRequest<AuthenticationResponse>;
