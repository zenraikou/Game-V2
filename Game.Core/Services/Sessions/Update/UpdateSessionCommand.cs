using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Update;

public record UpdateSessionCommand(SessionRequest Session) : IRequest<Unit>;
