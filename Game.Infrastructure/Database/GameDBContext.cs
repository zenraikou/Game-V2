using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Infrastructure.Database;

public class GameDBContext : DbContext
{
    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();
    }
}
