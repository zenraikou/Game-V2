using MediatR;

namespace Game.Core.Services.Players.Delete;

public record DeletePlayerCommand(Guid Id) : IRequest<Unit>;
