using System.Linq.Expressions;
using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Players.Get;

public record GetPlayerQuery(Expression<Func<Player, bool>> Expression) : IRequest<Player?>;
