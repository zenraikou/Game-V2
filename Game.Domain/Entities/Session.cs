namespace Game.Domain.Entities;

public record Session
{
    public required string JTI { get; set; }
    public required string Fingerprint { get; set; }
    public required DateTime Expiry { get; set; }
}
