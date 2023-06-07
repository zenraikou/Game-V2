using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Mappers;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Game.Core.Extensions;

public static class Extensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, Mapper>();

        services.AddSingleton<IExpressionMapper, ExpressionMapper>();

        return services;
    }
}
