namespace Game.Domain.Entities;

public class User
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime TokenExpiry { get; set; }
    public required DateTime TokenCreationStamp { get; set; }
    public DateTime CreationStamp { get; private init; } = DateTime.UtcNow;
}
