using System.Security.Claims;

namespace Game.Core.TempServices.PlayerClaim;

public interface IPlayerClaimService
{
    string? GetPlayerClaim(Func<Claim, bool> expression);
}
