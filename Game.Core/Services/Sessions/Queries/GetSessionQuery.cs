using System.Linq.Expressions;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Queries;

public record GetSessionQuery(Expression<Func<SessionRequest, bool>> Expression) : IRequest<SessionResponse?>;
