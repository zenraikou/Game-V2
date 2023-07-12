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
                    { new Guid("ffffffff-ffff-ffff-ffff-fffffffffff1"), new DateTime(2023, 7, 12, 13, 9, 6, 835, DateTimeKind.Utc).AddTicks(7918), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$W8ypLTkrD/s0JCNNKJQQnOEJSlUf2kKJrt9ggO.biTqEm3lWevkP.", 1, "kazuma" },
                    { new Guid("ffffffff-ffff-ffff-ffff-fffffffffff2"), new DateTime(2023, 7, 12, 13, 9, 7, 407, DateTimeKind.Utc).AddTicks(6922), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$bvSe6PD9kiWVI7HxrwlccOh0/.bu7DYxyDvK9.0L3Rc1USgXMK3WS", 1, "aqua" },
                    { new Guid("ffffffff-ffff-ffff-ffff-fffffffffff3"), new DateTime(2023, 7, 12, 13, 9, 7, 963, DateTimeKind.Utc).AddTicks(6599), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$nEV7DxtYOYoNCx8P02IIH.3CsqsmVBsb5cwVZUqmTRUV882uhk4se", 1, "megumin" },
                    { new Guid("ffffffff-ffff-ffff-ffff-fffffffffff4"), new DateTime(2023, 7, 12, 13, 9, 8, 517, DateTimeKind.Utc).AddTicks(9744), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$TL5Sq0pKrxO6bWnLN4CpJ.oU9XNi./xwyt3fCdxsU18zzmRGO5.TO", 1, "darkness" }
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
