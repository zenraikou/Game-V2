using ErrorOr;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.Fingerprinting;

public record FingerprintingCommand : IRequest<ErrorOr<Success>>;
