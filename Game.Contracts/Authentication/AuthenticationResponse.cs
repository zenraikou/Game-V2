namespace Game.Contracts.Authentication;

public record AuthenticationResponse
{
    public required string JWT { get; set; }
}
