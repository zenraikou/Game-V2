using MediatR;

namespace Game.Core.Services.Authentication.Logout;

public record LogoutCommand : IRequest<Unit>;
