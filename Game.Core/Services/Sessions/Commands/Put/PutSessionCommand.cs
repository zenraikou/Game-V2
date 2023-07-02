using ErrorOr;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Put;

public record PutSessionCommand(Guid Id, SessionRequest Session) : IRequest<ErrorOr<Updated>>;
