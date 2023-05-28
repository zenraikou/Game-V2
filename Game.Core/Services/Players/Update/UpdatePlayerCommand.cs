using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Update;

public record UpdatePlayerCommand(PlayerRequest Player) : IRequest<PlayerResponse>;
