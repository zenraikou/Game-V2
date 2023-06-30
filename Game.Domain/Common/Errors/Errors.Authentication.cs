using ErrorOr;
using Game.Domain.Common.Constants;

namespace Game.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Custom(
            type: ErrorCodes.BadRequest,
            code: "Authentication.InvalidCredentials",
            description: "Invalid credentials.");
    }
}
