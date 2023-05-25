using System.Security.Claims;

namespace Game.Core.TempServices.UserClaim;

public interface IUserClaimService
{
    string? GetUserClaim(Func<Claim, bool> expression);
}
