using Game.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddCommon(config);
        services.AddMediator();
        services.AddValidator();
        services.AddMappers();

        return services;
    }
}
