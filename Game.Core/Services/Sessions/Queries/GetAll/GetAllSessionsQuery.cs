using Game.Contracts.Session;
using MediatR;
using System.Linq.Expressions;

namespace Game.Core.Services.Sessions.Queries.GetAll;

public record GetAllSessionsQuery(Expression<Func<SessionRequest, bool>>? Expression = null) : IRequest<IEnumerable<SessionResponse>>;
