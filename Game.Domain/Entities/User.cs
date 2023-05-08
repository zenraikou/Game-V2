namespace Game.Domain.Entities;

public class User
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreationStamp { get; private init; } = DateTime.UtcNow;
}
