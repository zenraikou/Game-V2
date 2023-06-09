using ErrorOr;
using Game.Contracts.Player;
using MediatR;
using System.Linq.Expressions;

namespace Game.Core.Services.Players.Queries.Get;

public record GetPlayerQuery(Expression<Func<PlayerRequest, bool>> Expression) : IRequest<ErrorOr<PlayerResponse>>;
