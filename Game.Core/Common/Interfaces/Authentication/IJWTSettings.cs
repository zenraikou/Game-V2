namespace Game.Core.Common.Interfaces.Authentication;

public interface IJWTSettings
{
    string SectionName { get; }
    string Secret { get; }
    string Issuer { get; }
    string Audience { get; }
    int Expiry { get; }
}
