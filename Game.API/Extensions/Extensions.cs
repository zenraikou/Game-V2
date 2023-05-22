using Game.API.Middlewares;

namespace Game.API.Extensions;

public static class Extensions
{
    // Device Fingerprinting
    public static IServiceCollection AddFingerprinting(this IServiceCollection services)
    {
        services.AddScoped<FingerprintingMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseFingerprinting(this IApplicationBuilder app)
    {
        app.UseMiddleware<FingerprintingMiddleware>();
        return app;
    }

    // Global Exception Handling
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddScoped<ExceptionHandlingMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
