namespace Game.Contracts.Player;

public record PlayerRequest : PlayerBase
{
    public required string Password { get; set; }
}
