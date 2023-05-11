using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IJWTGenerator
{
    string GenerateToken(User user);
}
