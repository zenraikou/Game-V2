using FluentValidation;
using Game.Core.Common.Settings;
using Game.Core.TempServices.Authentication;
using Game.Core.TempServices.Fingerprinting;
using Game.Core.TempServices.Time;
using Game.Core.TempServices.TIme;
using Game.Core.TempServices.Token;
using Game.Core.TempServices.UserClaim;
using Game.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        
        services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.SectionName));
        services.AddSingleton<ITime, Time>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IUserClaimService, UserClaimService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IFingerprintingService, FingerprintingService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
