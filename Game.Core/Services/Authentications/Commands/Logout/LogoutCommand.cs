using ErrorOr;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Logout;

public record LogoutCommand : IRequest<ErrorOr<Success>>;
