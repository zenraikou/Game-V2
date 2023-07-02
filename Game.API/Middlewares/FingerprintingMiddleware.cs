using Game.API.Attributes;
using Game.Core.Services.Authentications.Commands.Fingerprinting;
using Game.Domain.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Middlewares;

public class FingerprintingMiddleware : IMiddleware
{
    private readonly IMediator _mediator;

    public FingerprintingMiddleware(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authorize = EndpointHasAttribute<AuthorizeAttribute>(context);
        var allowAnonymous = EndpointHasAttribute<AllowAnonymousAttribute>(context);
        var fingerprinting = EndpointHasAttribute<FingerprintingAttribute>(context);
        var noFingerprinting = EndpointHasAttribute<NoFingerprintingAttribute>(context);

        if (noFingerprinting)
        {
            fingerprinting = false;
        }

        if (authorize && !allowAnonymous && fingerprinting)
        {
            var response = await _mediator.Send(new FingerprintingCommand());

            if (response.IsError)
            {
                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Title = "Unauthorized.",
                    Status = ErrorCodes.Unauthorized
                };

                context.Response.StatusCode = problemDetails.Status.Value;
                await context.Response.WriteAsJsonAsync(problemDetails);
                return;
            }
        }

        await next(context);
    }

    public bool EndpointHasAttribute<T>(HttpContext context) where T : class
    {
        var endpoint = context.GetEndpoint();
        return endpoint?.Metadata.GetMetadata<T>() != null;
    }
}
