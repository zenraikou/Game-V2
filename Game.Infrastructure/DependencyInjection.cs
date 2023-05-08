using Game.Core.Common.Interfaces.Authentication;
using Game.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IJWTGenerator, JWTGenerator>();
        return services;
    }
}
