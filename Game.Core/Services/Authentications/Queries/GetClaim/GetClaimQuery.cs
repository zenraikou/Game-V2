using ErrorOr;
using MediatR;
using System.Security.Claims;

namespace Game.Core.Services.Authentications.Queries.GetClaim;

public record GetClaimQuery(Func<Claim, bool> Expression, string? JWT = null) : IRequest<ErrorOr<string>>;
