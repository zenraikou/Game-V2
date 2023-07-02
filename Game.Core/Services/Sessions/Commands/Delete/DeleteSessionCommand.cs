using ErrorOr;
using MediatR;

namespace Game.Core.Services.Sessions.Commands.Delete;

public record DeleteSessionCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
