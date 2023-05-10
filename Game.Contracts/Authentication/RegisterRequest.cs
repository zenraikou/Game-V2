using Game.Domain.Entities;

namespace Game.Contracts.Authentication;

public record RegisterRequest
{
    public required string Handle { get; set; }
    public required string Name { get; set; }
    public required string UniqueName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required Role Role { get; set; }
}
