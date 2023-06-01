using System.Linq.Expressions;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Get;

public record GetSessionQuery(Expression<Func<SessionRequest, bool>> Expression) : IRequest<SessionResponse?>;
