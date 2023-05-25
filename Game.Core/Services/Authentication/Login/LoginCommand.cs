using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentication.Login;

public record LoginCommand(LoginRequest User) : IRequest<LoginRequest>;
