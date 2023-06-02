using System.Security.Claims;
using MediatR;

namespace Game.Core.Services.Claims;

public record GetClaimCommand(Func<Claim, bool> Expression) : IRequest<string>;
