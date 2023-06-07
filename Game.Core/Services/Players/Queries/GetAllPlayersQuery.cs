using System.Linq.Expressions;
using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Queries;

public record GetAllPlayersQuery(Expression<Func<PlayerRequest, bool>>? Expression = null) : IRequest<IEnumerable<PlayerResponse>>;
