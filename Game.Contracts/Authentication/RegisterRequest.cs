namespace Game.Contracts.Authentication;

public record RegisterRequest
{
    public Guid Id { get; private init; }
    public required string Name { get; set; }
    public required string Handle { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
