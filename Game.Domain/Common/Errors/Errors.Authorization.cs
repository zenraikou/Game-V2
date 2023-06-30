using ErrorOr;
using Game.Domain.Common.Constants;

namespace Game.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authorization
    {
        public static Error Unauthorized => Error.Custom(
            type: ErrorCodes.Unauthorized,
            code: "Authentication.Unauthorized",
            description: "Unauthorized.");
    }
}
