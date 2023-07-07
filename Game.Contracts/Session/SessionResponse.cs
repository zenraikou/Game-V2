namespace Game.Contracts.Session;

public record SessionResponse : SessionBase
{
    public Guid Id { get; set; } /* JTI */
}
