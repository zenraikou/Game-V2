using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Update;

public record UpdatePlayerCommand(Guid Id, PlayerRequest Player) : IRequest<Unit>;
