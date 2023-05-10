namespace Game.Domain.Entities;

public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expiry { get; set; }
    public DateTime CreationStamp { get; private init; } = DateTime.UtcNow;
}
