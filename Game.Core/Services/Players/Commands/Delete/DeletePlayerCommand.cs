using ErrorOr;
using MediatR;

namespace Game.Core.Services.Players.Commands.Delete;

public record DeletePlayerCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
