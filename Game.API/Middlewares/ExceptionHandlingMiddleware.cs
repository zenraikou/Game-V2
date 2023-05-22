using System.Net;
using System.Text.Json;
using Game.Core.Exceptions;

namespace Game.API.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandling(context, ex);
        }
    }

    public async Task ExceptionHandling(HttpContext context, Exception ex)
    {
        HttpStatusCode code;
        var message = string.Empty;

        if (ex.GetType() == typeof(UnimplementedException))
        {
            code = HttpStatusCode.NotImplemented;
            message = ex.Message;
        }
        else if (ex.GetType() == typeof(InvalidKeyException))
        {
            code = HttpStatusCode.NotFound;
            message = ex.Message;
        }
        else if (ex.GetType() == typeof(NotFoundException))
        {
            code = HttpStatusCode.NotFound;
            message = ex.Message;
        }
        else if (ex.GetType() == typeof(BadRequestException))
        {
            code = HttpStatusCode.BadRequest;
            message = ex.Message;
        }
        else if (ex.GetType() == typeof(UnauthorizedException))
        {
            code = HttpStatusCode.Unauthorized;
            message = ex.Message;
        }
        else
        {
            code = HttpStatusCode.InternalServerError;
            message = ex.Message;
        }

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json+problem";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(result);
    }
}
