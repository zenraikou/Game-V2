using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public static class Extensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasData(
            new Player
            {
                Handle = "kazuma",
                Name = "Kazuma",
                UniqueName = "kazuma",
                Email = "kazuma@konosuba.gg",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("?Ok4"),
                Role = Role.Player
            },
            new Player
            {
                Handle = "aqua",
                Name = "Aqua",
                UniqueName = "aqua",
                Email = "aqua@konosuba.gg",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("?Ok4"),
                Role = Role.Player
            },
            new Player
            {
                Handle = "megumin",
                Name = "Megumin",
                UniqueName = "megumin",
                Email = "megumin@konosuba.gg",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("?Ok4"),
                Role = Role.Player
            },
            new Player
            {
                Handle = "darkness",
                Name = "Darkness",
                UniqueName = "darkness",
                Email = "darkness@konosuba.gg",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("?Ok4"),
                Role = Role.Player
            });
    }
}
