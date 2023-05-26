using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Player.GetAll;

public record GetAllPlayersQuery : IRequest<IEnumerable<PlayerResponse>>;
