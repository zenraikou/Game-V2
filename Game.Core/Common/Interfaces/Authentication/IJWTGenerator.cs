namespace Game.Core.Common.Interfaces.Authentication;

public interface IJWTGenerator
{
    string GenerateToken(Guid id, string name, string uniqueName, string email);
}
