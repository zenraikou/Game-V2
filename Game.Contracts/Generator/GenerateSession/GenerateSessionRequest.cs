namespace Game.Contracts.Generator.GenerateSession;

public record GenerateSessionRequest
{
    public required string JWT { get; set; }
}
