namespace Game.Contracts.Authentication;

public record AuthenticationResponse
{
    public required string Token { get; set; }
}
