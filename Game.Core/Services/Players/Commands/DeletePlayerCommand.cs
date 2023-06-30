using ErrorOr;
using MediatR;

namespace Game.Core.Services.Players.Commands;

public record DeletePlayerCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
