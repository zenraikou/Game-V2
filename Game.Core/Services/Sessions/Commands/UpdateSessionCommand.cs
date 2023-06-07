using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Commands;

public record UpdateSessionCommand(Guid Id, SessionRequest Session) : IRequest<Unit>;
