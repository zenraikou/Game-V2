namespace Game.Contracts.Authentication;

public record LoginRequest
{
    public required string UniqueName { get; set; }
    public required string Password { get; set; }
}
