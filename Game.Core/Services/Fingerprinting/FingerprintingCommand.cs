using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Fingerprinting;

public record FingerprintingCommand(SessionRequest Session) : IRequest<SessionResponse>;
