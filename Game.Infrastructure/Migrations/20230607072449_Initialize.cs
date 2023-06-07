using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreationStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fingerprint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreationStamp", "Email", "Handle", "Name", "PasswordHash", "Role", "UniqueName" },
                values: new object[,]
                {
                    { new Guid("02749775-f2d0-417b-b5dd-6112c385f7bc"), new DateTime(2023, 6, 7, 7, 24, 47, 689, DateTimeKind.Utc).AddTicks(7414), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$Cuu/EP6htnyxxRU8.qTMzOxvlfZh9vgt2qkYOCcCYHCjX.Xj1iFwC", 1, "megumin" },
                    { new Guid("0c3af466-cae1-40bf-8c10-1faffbd39d62"), new DateTime(2023, 6, 7, 7, 24, 48, 182, DateTimeKind.Utc).AddTicks(2122), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$I1kGORQ3fflGss68DZ6vaejnA7ViUhnAI1pv4CYwZuTNpMUz81HN2", 1, "darkness" },
                    { new Guid("696ef631-f036-4b4c-b060-30ce19c9b892"), new DateTime(2023, 6, 7, 7, 24, 47, 196, DateTimeKind.Utc).AddTicks(7444), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$YK77jw/wejV45dr/UaEG1.ZV.UwwyOMee0t8VfJEVQgZZRyGKqBGi", 1, "aqua" },
                    { new Guid("9c52aea2-241e-4821-b866-35ebd1a4aa90"), new DateTime(2023, 6, 7, 7, 24, 46, 707, DateTimeKind.Utc).AddTicks(9903), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$xJDWgS8MPpEvKowvYf1gOuIVgLqDmtkQ6njhg.7iHqYPt7B82YaLq", 1, "kazuma" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
