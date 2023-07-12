namespace Game.Contracts.Player;

public record PlayerResponse : PlayerBase
{
    public required Guid Id { get; set; }
    internal string? PasswordHash { get; set; }
    public required DateTime CreationStamp { get; set; }
}
