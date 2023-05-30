namespace Game.Contracts.Player;

public record PlayerResponse
{
    public Guid Id { get; private init; }
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
}
