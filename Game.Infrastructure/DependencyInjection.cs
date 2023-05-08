using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Game.Infrastructure.Authentication;
using Game.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.SectionName));
        services.AddSingleton<IJWTGenerator, JWTGenerator>();
        services.AddSingleton<IDateTImeProvider, DateTimeProvider>();
        return services;
    }
}
