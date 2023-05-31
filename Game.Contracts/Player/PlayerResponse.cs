namespace Game.Contracts.Player;

public record PlayerResponse : PlayerBase
{
    public required string PasswordHash { get; set; }
}
