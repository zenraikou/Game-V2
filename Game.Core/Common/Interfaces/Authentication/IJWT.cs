namespace Game.Core.Common.Interfaces.Authentication;

public interface IJWT
{
    string GenerateToken(Guid userId, string firstName, string lastName);
}
