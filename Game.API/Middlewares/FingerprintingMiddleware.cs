using Game.API.Attributes;
using Game.Core.Services.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Middlewares;

public class FingerprintingMiddleware : IMiddleware
{
    private readonly ILogger<FingerprintingMiddleware> _logger;
    private readonly IMediator _mediator;

    public FingerprintingMiddleware(ILogger<FingerprintingMiddleware> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authorize = EndpointHasAttribute<AuthorizeAttribute>(context);
        var fingerprinting = EndpointHasAttribute<FingerprintingAttribute>(context);
        var noFingerprinting = EndpointHasAttribute<NoFingerprintingAttribute>(context);

        if (noFingerprinting)
        {
            fingerprinting = false;
        }

        if (authorize && fingerprinting)
        {
            var fingerprintingCommand = new FingerprintingCommand();
            await _mediator.Send(fingerprintingCommand);
        }

        await next(context);
    }

    public bool EndpointHasAttribute<T>(HttpContext context) where T : class
    {
        var endpoint = context.GetEndpoint();
        return endpoint?.Metadata.GetMetadata<T>() is not null;
    }
}
