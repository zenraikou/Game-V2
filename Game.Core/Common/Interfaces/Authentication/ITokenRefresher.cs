using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface ITokenRefresher
{
    RefreshToken RefreshToken();
}
