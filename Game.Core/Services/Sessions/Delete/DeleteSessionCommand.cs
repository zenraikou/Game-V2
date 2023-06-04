using MediatR;

namespace Game.Core.Services.Sessions.Delete;

public record DeleteSessionCommand(string Id) : IRequest<Unit>;
