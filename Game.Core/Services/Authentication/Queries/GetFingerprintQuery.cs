using MediatR;

namespace Game.Core.Services.Authentication.Queries;

public record GetFingerprintQuery : IRequest<string>;
