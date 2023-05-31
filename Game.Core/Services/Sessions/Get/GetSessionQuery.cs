using System.Linq.Expressions;
using Game.Contracts.Session;
using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Sessions.Get;

public record GetSessionQuery(Expression<Func<Session, bool>> Expression) : IRequest<Session?>;
