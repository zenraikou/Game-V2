namespace Game.Domain.Entities;

public record Session
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string JTI { get; set; }
    public required string Fingerprint { get; set; }
    public required DateTime Expiry { get; set; }
}
