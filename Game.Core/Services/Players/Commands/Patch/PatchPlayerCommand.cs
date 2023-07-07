using ErrorOr;
using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Commands.Patch;

public record PatchPlayerCommand(Guid Id, PlayerRequest Player) : IRequest<ErrorOr<Updated>>;
