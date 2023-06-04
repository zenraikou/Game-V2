namespace Game.Contracts.Player;

public record PlayerRequest : PlayerBase
{
    public Guid Id { get; internal set; }
    public required string Password { get; set; }
}
