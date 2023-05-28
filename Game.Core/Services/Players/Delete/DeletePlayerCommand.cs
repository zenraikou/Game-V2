using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Delete;

public record DeletePlayerCommand(PlayerRequest Player) : IRequest<PlayerResponse>;
