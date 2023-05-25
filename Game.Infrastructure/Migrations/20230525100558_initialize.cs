using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialize : Migration
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
                    JTI = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    { new Guid("6994ed90-de07-4b48-aab9-35ba7a993242"), new DateTime(2023, 5, 25, 10, 5, 56, 385, DateTimeKind.Utc).AddTicks(1765), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$OuK6MmyqyBfB07LgbhUtM.92LjB/X6ZYjOigtVpCOzuX8.t6mHI6K", 1, "kazuma" },
                    { new Guid("848473bf-884d-4681-86fb-a1ca3ef7f8f7"), new DateTime(2023, 5, 25, 10, 5, 56, 867, DateTimeKind.Utc).AddTicks(7135), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$GyRI3UWjZDzthpMGIE0B4.kCfXkqDHXIGZwi8/TQcWqKsYErYksLa", 1, "aqua" },
                    { new Guid("b01740c0-8ce8-4bef-ac8f-2d244084772b"), new DateTime(2023, 5, 25, 10, 5, 57, 891, DateTimeKind.Utc).AddTicks(8476), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$OyioTJLxr8fa8i5Tcgm6XOXmSUasRV3jSb1CcB/BtrW7YZ7xmrlF6", 1, "darkness" },
                    { new Guid("bcb0c1f7-1431-4a35-b582-86ed9b0fbac7"), new DateTime(2023, 5, 25, 10, 5, 57, 386, DateTimeKind.Utc).AddTicks(376), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$5e34uy3KfODp38oyvYg/Auj63o/wAGFWbYZ0zDITmwkkIRASpNsAO", 1, "megumin" }
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
