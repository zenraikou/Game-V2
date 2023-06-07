namespace Game.Contracts.Session;

public record SessionRequest : SessionBase
{
    internal Guid Id { get; set; }
}
