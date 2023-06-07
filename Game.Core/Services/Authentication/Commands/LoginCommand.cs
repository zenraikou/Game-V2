using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record LoginCommand(LoginRequest Login) : IRequest<AuthenticationResponse>;
