using System.Linq.Expressions;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Queries;

public record GetAllSessionsQuery(Expression<Func<SessionRequest, bool>>? Expression = null) : IRequest<IEnumerable<SessionResponse>>;
