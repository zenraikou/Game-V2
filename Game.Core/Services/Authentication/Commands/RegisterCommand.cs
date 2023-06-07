using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record RegisterCommand(RegisterRequest Register) : IRequest<AuthenticationResponse>;
