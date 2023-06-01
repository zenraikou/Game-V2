using System.Linq.Expressions;
using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.Players.Get;

public record GetPlayerQuery(Expression<Func<PlayerRequest, bool>> Expression) : IRequest<PlayerResponse?>;
