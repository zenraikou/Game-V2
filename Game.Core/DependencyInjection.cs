using FluentValidation;
using Game.Core.Common.Interfaces.Mappers;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Settings;
using Game.Core.Common.Time;
using Game.Core.Extensions;
using Game.Core.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddMappers();

        services.AddSingleton<IExpressionMapper, ExpressionMapper>();
        
        services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.SectionName));
        services.AddSingleton<ITime, Time>();
        services.AddHttpContextAccessor();

        return services;
    }
}
