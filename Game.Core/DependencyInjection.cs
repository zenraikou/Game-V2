using ErrorOr;
using FluentValidation;
using Game.Contracts.Authentication;
using Game.Core.Behaviors;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Settings;
using Game.Core.Common.Time;
using Game.Core.Extensions;
using Game.Core.Services.Authentication.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.SectionName));
        services.AddSingleton<ITime, Time>();
        services.AddHttpContextAccessor();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddMappers();

        return services;
    }
}
