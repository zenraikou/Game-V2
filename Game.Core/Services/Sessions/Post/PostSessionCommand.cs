using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Post;

public record PostSessionCommand(SessionRequest Session) : IRequest<SessionResponse>;
