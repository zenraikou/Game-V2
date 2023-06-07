using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record FingerprintingCommand : IRequest<Unit>;
