using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Post;

public record PostSessionCommand(SessionRequest Session) : IRequest<SessionResponse>;
