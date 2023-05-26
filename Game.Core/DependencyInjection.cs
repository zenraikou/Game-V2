using FluentValidation;
using Game.Core.Common.Settings;
using Game.Core.TempServices.Authentication;
using Game.Core.TempServices.Fingerprinting;
using Game.Core.TempServices.Time;
using Game.Core.TempServices.TIme;
using Game.Core.TempServices.JWT;
using Game.Core.TempServices.PlayerClaim;
using Game.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MapsterMapper;

namespace Game.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddSingleton<IMapper, Mapper>();
        
        services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.SectionName));
        services.AddSingleton<ITime, Time>();
        services.AddSingleton<IJWTService, JWTService>();
        services.AddScoped<IPlayerClaimService, PlayerClaimService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IFingerprintingService, FingerprintingService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
