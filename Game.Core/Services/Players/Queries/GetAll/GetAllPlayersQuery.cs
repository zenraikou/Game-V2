using Game.Contracts.Player;
using MediatR;
using System.Linq.Expressions;

namespace Game.Core.Services.Players.Queries.GetAll;

public record GetAllPlayersQuery(Expression<Func<PlayerRequest, bool>>? Expression = null) : IRequest<IEnumerable<PlayerResponse>>;
