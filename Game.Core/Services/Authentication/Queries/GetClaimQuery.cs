using MediatR;
using System.Security.Claims;

namespace Game.Core.Services.Authentication.Queries;

public record GetClaimQuery(Func<Claim, bool> Expression, string? JWT = null) : IRequest<string>;
