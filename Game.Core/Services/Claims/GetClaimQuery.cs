using System.Security.Claims;
using MediatR;

namespace Game.Core.Services.Claims;

public record GetClaimQuery(Func<Claim, bool> Expression, string? JWT = null) : IRequest<string>;
