using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Fingerprinting;

public record CheckFingerprintCommand(Session Session) : IRequest<Session>;
