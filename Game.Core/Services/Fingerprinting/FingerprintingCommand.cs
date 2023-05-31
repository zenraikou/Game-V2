using MediatR;

namespace Game.Core.Services.Fingerprinting;

public record FingerprintingCommand() : IRequest<Unit>;
