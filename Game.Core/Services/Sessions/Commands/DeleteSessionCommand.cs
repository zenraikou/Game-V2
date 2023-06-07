using MediatR;

namespace Game.Core.Services.Sessions.Commands;

public record DeleteSessionCommand(Guid Id) : IRequest<Unit>;
