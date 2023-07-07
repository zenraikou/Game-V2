using ErrorOr;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Patch;

public record PatchSessionCommand(Guid Id, SessionRequest Session) : IRequest<ErrorOr<Updated>>;
