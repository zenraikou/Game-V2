namespace Game.Contracts.Player;

public record PlayerRequest : PlayerBase
{
    internal Guid Id { get; set; }
    public required string Password { get; set; }
}
