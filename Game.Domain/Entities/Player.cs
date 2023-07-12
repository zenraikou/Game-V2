namespace Game.Domain.Entities;

public record Player
{
    public required Guid Id { get; set; }
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public required DateTime CreationStamp { get; set; }
}
