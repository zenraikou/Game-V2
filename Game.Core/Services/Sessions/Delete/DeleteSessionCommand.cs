using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public record DeleteSessionCommand(string JTI) : IRequest<Unit>;
