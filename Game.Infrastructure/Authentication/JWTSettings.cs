namespace Game.Infrastructure.Authentication;

public class JWTSettings
{
    public const string SectionName = "JWTSettings";
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int Expiry { get; init; }
}
