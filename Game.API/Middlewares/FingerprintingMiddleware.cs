using Game.API.Attributes;
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
        var fingerprint = context.Request.Headers["Fingerprint"].ToString();

        if (endpoint?.Metadata.GetMetadata<FingerprintingAttribute>() is not null)
        {
            if (fingerprint is null) throw new Exception("Fingerprint is required.");
        }

        if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is not null)
        {
            await _fingerprintingService.Validate();
        }

        bool isAuthorized = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is not null; // temporary
        Console.WriteLine($"Authorized Endpoint: {isAuthorized}"); // temporary
        await next(context);
    }
}
