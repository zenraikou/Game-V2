using Game.Core.Common.Interfaces.Persistence;
using Game.Infrastructure.Database;
using Game.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<GameDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("GameConnection"));
        });

        services.AddScoped<ISessionRepository, MockSessionRepository>();

        return services;
    }
}
