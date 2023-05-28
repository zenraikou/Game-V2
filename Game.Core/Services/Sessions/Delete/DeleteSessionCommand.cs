using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public record DeleteSessionCommand(SessionRequest Session) : IRequest<SessionResponse>;
