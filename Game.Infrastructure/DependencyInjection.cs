using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GameContext>(options => 
        {
            options.UseSqlServer(
                configuration.GetConnectionString("GameConnection"),
                b => b.MigrationsAssembly("Game.Infrastructure"));
        });

        return services;
    }
}