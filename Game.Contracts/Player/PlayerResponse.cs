namespace Game.Contracts.Player;

public record PlayerResponse : PlayerBase
{
    public Guid Id { get; set; }
    public required string PasswordHash { get; set; }
}
