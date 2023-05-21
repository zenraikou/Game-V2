using System.Security.Claims;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IUserService
{
    string? GetUserClaim(Func<Claim, bool> expression);
}
