using Game.API.Attributes;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Middlewares;

public class FingerprintingMiddleware : IMiddleware
{
    private readonly IFingerprintingService _fingerprintingService;
    private readonly ILogger<FingerprintingMiddleware> _logger;

    public FingerprintingMiddleware(IFingerprintingService fingerprintingService, ILogger<FingerprintingMiddleware> logger)
    {
        _fingerprintingService = fingerprintingService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var fingerprint = context.Request.Headers["Fingerprint"].ToString();

        var fingerprinting = endpoint?.Metadata.GetMetadata<FingerprintingAttribute>() is not null;
        var noFingerprinting = endpoint?.Metadata.GetMetadata<NoFingerprintingAttribute>() is not null;
        var authorize = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is not null;

        if (noFingerprinting is false)
        {
            if (fingerprinting && fingerprint is null) throw new UnauthorizedException("Fingerprint is required.");
        }

        if (authorize)
        {
            await _fingerprintingService.Validate();
        }

        _logger.LogInformation($"Authorization Header Required: {authorize}"); // temporary
        _logger.LogInformation($"Fingerprinting Is Required: {!noFingerprinting}"); // temporary
        await next(context);
    }
}
