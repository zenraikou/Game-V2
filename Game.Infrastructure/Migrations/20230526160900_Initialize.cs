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
                    { new Guid("2d7a604f-5b2e-49b1-a5af-2a05772ad823"), new DateTime(2023, 5, 26, 16, 8, 59, 299, DateTimeKind.Utc).AddTicks(3729), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$7n57nZGS3CeL6U7aTKHE0e7vcX0BG4WxpHFNDk0PWS9DxIxrm/BPS", 1, "megumin" },
                    { new Guid("2f19c652-8a18-4980-950f-0222d1dbe301"), new DateTime(2023, 5, 26, 16, 8, 59, 790, DateTimeKind.Utc).AddTicks(4016), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$uMX8rpWZJ/.YryYt8MY..e3FiPHgn120GhtHEReCxEA0brkybhmlK", 1, "darkness" },
                    { new Guid("a08c82e1-9fe3-478f-8ace-c88fed14ee71"), new DateTime(2023, 5, 26, 16, 8, 58, 819, DateTimeKind.Utc).AddTicks(5142), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$Ic1lVC.SwCxoO1c.4LdyOOTw5/g025W8M2pdqXEF.Y3BMGjMxIULW", 1, "aqua" },
                    { new Guid("b2b92e20-bc64-431b-98d1-c5b41c9d7396"), new DateTime(2023, 5, 26, 16, 8, 58, 334, DateTimeKind.Utc).AddTicks(4153), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$dcPkpp7cHUMXof5wkx5o4OdK/wpGoRYHNBRvFK3x6Her1bKGq9n0a", 1, "kazuma" }
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
