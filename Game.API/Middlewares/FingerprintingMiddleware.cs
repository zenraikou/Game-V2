using Game.Core.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Middlewares;

public class FingerprintingMiddleware : IMiddleware
{
    private readonly IFingerprintingService _fingerprintingService;

    public FingerprintingMiddleware(IFingerprintingService fingerprintingService)
    {
        _fingerprintingService = fingerprintingService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is null) throw new Exception("Endpoint not found.");

        var authorizeAttributes = endpoint.Metadata.GetOrderedMetadata<AuthorizeAttribute>();
        bool isAuthorized = authorizeAttributes.Any();
        Console.WriteLine($"Authorized Endpoint: {isAuthorized}");

        if (isAuthorized)
        {
            await _fingerprintingService.Validate();
        }

        await next.Invoke(context);
    }
}
