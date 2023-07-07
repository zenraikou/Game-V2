namespace Game.Contracts.Session;

public record SessionBase
{
    public required string Fingerprint { get; set; }
    public required DateTime Expiry { get; set; }
}
