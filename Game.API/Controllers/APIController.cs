using ErrorOr;
using Game.API.Common.Constants;
using Game.Domain.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Game.API.Controllers;

[ApiController]
public class APIController : ControllerBase
{
    [Route("error")]
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items[HTTPItem.Errors] = errors;
        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var code = error.NumericType switch
        {
            ErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
            ErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCodes.NotFound => StatusCodes.Status404NotFound,
            ErrorCodes.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: code, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var dictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            dictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(dictionary);
    }

}
