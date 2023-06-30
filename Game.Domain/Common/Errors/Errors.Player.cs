using ErrorOr;
using Game.Domain.Common.Constants;

namespace Game.Domain.Common.Errors;

public static partial class Errors
{
    public static class Player
    {
        public static Error NotFound => Error.Custom(
            type: ErrorCodes.NotFound,
            code: "Player.NotFound",
            description: "Player not found.");

        public static Error DuplicateEmail => Error.Custom(
            type: ErrorCodes.Conflict,
            code: "Player.DuplicateEmail",
            description: "Email is alredy in use.");
    }
}
