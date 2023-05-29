using Game.Contracts.Player;

namespace Game.Contracts.Generator.GenerateJWT;

public record GenerateJWTRequest
{
    public required PlayerRequest Player { get; set; }
}
