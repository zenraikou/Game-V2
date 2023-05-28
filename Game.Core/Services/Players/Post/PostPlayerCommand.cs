using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Post;

public record PostPlayerCommand(PlayerRequest Player) : IRequest<PlayerResponse>;
