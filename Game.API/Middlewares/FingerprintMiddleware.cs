using Game.Core.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Middlewares;

public class FingerprintMiddleware : IMiddleware
{
    private readonly IFingerprintService _fingerprintService;

    public FingerprintMiddleware(IFingerprintService fingerprintService)
    {
        _fingerprintService = fingerprintService;
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
            await _fingerprintService.ValidateFingerprint();
        }

        await next.Invoke(context);
    }
}
