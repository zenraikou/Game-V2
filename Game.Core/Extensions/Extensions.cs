using FluentValidation;
using Game.Core.Behaviors;
using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Settings;
using Game.Core.Common.Time;
using Game.Core.Mappers;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Game.Core.Extensions;

public static class Extensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<JWTSettings>(config.GetSection(JWTSettings.SectionName));
        services.AddSingleton<ITime, Time>();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
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
