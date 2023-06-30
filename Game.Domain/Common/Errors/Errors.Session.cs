using ErrorOr;
using Game.Domain.Common.Constants;

namespace Game.Domain.Common.Errors;

public static partial class Errors
{
    public static class Session
    {
        public static Error NotFound => Error.Custom(
            type: ErrorCodes.NotFound,
            code: "Session.NotFound",
            description: "Session not found.");
    }
}
