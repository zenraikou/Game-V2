using Game.API.Extensions;

namespace Game.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddCommon();
        services.AddFingerprinting();
        services.AddAuth(config);

        return services;
    }

    public static IApplicationBuilder UseAPI(this IApplicationBuilder app)
    {
        app.UseCommon();
        app.UseErrorHandler();
        app.UseFingerprinting();
        app.UseAuth();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    
        return app;
    }
}
