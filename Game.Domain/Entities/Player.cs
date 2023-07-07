namespace Game.Domain.Entities;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public DateTime CreationStamp { get; private init; } = DateTime.UtcNow;
}
