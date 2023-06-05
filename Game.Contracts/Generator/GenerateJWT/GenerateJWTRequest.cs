namespace Game.Contracts.Generator.GenerateJWT;

public record GenerateJWTRequest
{
    public required string Id { get; set; }
    public required string Role { get; set; }
    public string? JTI { get; set; }
}
