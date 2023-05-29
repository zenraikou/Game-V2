using Game.Contracts.Session;

namespace Game.Contracts.Generator.GenerateSession;

public record GenerateSessionResponse
{
    public required SessionResponse Session { get; set; }
}
