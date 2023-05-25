using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Infrastructure.Database;

public class GameDBContext : DbContext
{
    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

    public virtual DbSet<Player> Players => Set<Player>();
    public virtual DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();
    }
}
