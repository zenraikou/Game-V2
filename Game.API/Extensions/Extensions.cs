using Game.API.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Game.API.Extensions;

public static class Extensions
{
    /* Add */
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();
        services.AddRouting(options => options.LowercaseUrls = true);

        return services;
    }

    public static IServiceCollection AddFingerprinting(this IServiceCollection services)
    {
        services.AddScoped<FingerprintingMiddleware>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTSettings:Secret").Value!)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    /* Use */
    public static IApplicationBuilder UseCommon(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();

        return app;
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler("/error");

        return app;
    }

    public static IApplicationBuilder UseFingerprinting(this IApplicationBuilder app)
    {
        app.UseMiddleware<FingerprintingMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
