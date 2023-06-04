namespace Game.Contracts.Player;

public record PlayerBase
{
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}
