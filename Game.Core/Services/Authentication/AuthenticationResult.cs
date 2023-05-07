namespace Game.Core.Services.Authentication;

public record AuthenticationResult
{
    public Guid Id { get; private init; }
    public required string Name { get; set; }
    public required string Handle { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}
