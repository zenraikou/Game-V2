using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Infrastructure.Extensions;

public static class Extensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasData(
            new Player
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffff1"),
                Handle = "kazuma",
                Name = "Kazuma",
                UniqueName = "kazuma",
                Email = "kazuma@konosuba.gg",
                PasswordHash = Cypher.HashPassword("?Ok4"),
                Role = Role.Player,
                CreationStamp = DateTime.UtcNow
            },
            new Player
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffff2"),
                Handle = "aqua",
                Name = "Aqua",
                UniqueName = "aqua",
                Email = "aqua@konosuba.gg",
                PasswordHash = Cypher.HashPassword("?Ok4"),
                Role = Role.Player,
                CreationStamp = DateTime.UtcNow
            },
            new Player
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffff3"),
                Handle = "megumin",
                Name = "Megumin",
                UniqueName = "megumin",
                Email = "megumin@konosuba.gg",
                PasswordHash = Cypher.HashPassword("?Ok4"),
                Role = Role.Player,
                CreationStamp = DateTime.UtcNow
            },
            new Player
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffff4"),
                Handle = "darkness",
                Name = "Darkness",
                UniqueName = "darkness",
                Email = "darkness@konosuba.gg",
                PasswordHash = Cypher.HashPassword("?Ok4"),
                Role = Role.Player,
                CreationStamp = DateTime.UtcNow
            });
    }
}
