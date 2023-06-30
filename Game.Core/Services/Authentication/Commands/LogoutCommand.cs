using ErrorOr;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record LogoutCommand : IRequest<ErrorOr<Success>>;
