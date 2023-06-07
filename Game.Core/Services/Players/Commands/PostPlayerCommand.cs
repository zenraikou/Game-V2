using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Commands;

public record PostPlayerCommand(PlayerRequest Player) : IRequest<PlayerResponse>;
