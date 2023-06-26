using ErrorOr;

namespace Game.Domain.Common.Errors;

public static partial class Errors
{
    public static class Player
    {
        public static Error NotFound => Error.NotFound(
            code: "Player.NotFound",
            description: "Player not found.");

        public static Error DuplicateEmail => Error.Conflict(
            code: "Player.DuplicateEmail",
            description: "Email is alredy in use.");
    }
}
