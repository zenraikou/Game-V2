namespace Game.Domain.Entities;

public record RefreshToken
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
    public required DateTime Expiry { get; set; }
}
