namespace Game.Contracts.Player;

public record PlayerResponse : PlayerBase
{
    public Guid Id { get; set; }
    internal string? PasswordHash { get; set; }
}
