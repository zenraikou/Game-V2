using ErrorOr;
using Game.Contracts.Session;
using MediatR;
using System.Linq.Expressions;

namespace Game.Core.Services.Sessions.Queries;

public record GetSessionQuery(Expression<Func<SessionRequest, bool>> Expression) : IRequest<ErrorOr<SessionResponse>>;
