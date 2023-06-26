using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Controllers;

[ApiController]
public class APIController : ControllerBase
{
    [Route("error")]
    public IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];

        var code = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: code, title: firstError.Description);
    }
}
