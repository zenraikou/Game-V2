using ErrorOr;
using MediatR;

namespace Game.Core.Services.Authentications.Queries.GetFingerprint;

public record GetFingerprintQuery : IRequest<ErrorOr<string>>;
