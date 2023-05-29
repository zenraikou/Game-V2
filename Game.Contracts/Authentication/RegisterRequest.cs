using Game.Contracts.Player;

namespace Game.Contracts.Authentication;

public record RegisterRequest
{
    public required PlayerRequest Player { get; set; }
    public required string Password { get; set; }
}
