using ErrorOr;
using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Commands.Put;

public record PutPlayerCommand(Guid Id, PlayerRequest Player) : IRequest<ErrorOr<Updated>>;
