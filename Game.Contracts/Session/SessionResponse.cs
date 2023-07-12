namespace Game.Contracts.Session;

public record SessionResponse : SessionBase
{
    public required Guid Id { get; set; } /* JTI */
}
