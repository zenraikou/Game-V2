namespace Game.Contracts.Authentication;

public record AuthenticationResponse
{
    public Guid Id { get; private init; }
    public required string Name { get; set; }
    public required string Handle { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}
