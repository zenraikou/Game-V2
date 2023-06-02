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
                    JTI = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fingerprint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.JTI);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreationStamp", "Email", "Handle", "Name", "PasswordHash", "Role", "UniqueName" },
                values: new object[,]
                {
                    { new Guid("1559905d-71f3-4c98-a69f-8fd7cf8d8a53"), new DateTime(2023, 6, 2, 9, 40, 28, 342, DateTimeKind.Utc).AddTicks(2156), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$R0HMtACMOOg.ST4K8gLAvOfHf9/MEe9CLJw3Sns4Niykiuw6gqoYG", 1, "aqua" },
                    { new Guid("441c386a-4dff-47fd-827a-3310bdeb8074"), new DateTime(2023, 6, 2, 9, 40, 29, 475, DateTimeKind.Utc).AddTicks(3605), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$KLWojnpLB4fS.woDN8/Cb.6IoVQJrdShkvwZz81dg3lxen7WGbsBO", 1, "darkness" },
                    { new Guid("93cbf201-8dd8-4306-a542-1e3e91d1b356"), new DateTime(2023, 6, 2, 9, 40, 27, 849, DateTimeKind.Utc).AddTicks(8290), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$Ql/mnmJyyGu.zbU5VAE0Lemq57t89NjRKby6EZac57GOmq9ad2H0m", 1, "kazuma" },
                    { new Guid("a195bc36-a9fe-40f9-baaf-be5b100d4a78"), new DateTime(2023, 6, 2, 9, 40, 28, 835, DateTimeKind.Utc).AddTicks(9378), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$j54jpz3wZsKWtgm0zJjK1uUck28MPY2ny7.KYIxV8c8R//GFq9O3a", 1, "megumin" }
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
