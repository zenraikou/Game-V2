using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Update;

public record UpdateSessionCommand(Guid Id, SessionRequest Session) : IRequest<Unit>;
