using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public record DeleteSessionCommand(Guid Id) : IRequest<Unit>;
