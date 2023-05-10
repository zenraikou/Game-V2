using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Game.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddControllers();
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings:Secret").Value!))
            };
        });

        return services;
    }

    public static IApplicationBuilder UseAPI(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        return app;
    }
}
