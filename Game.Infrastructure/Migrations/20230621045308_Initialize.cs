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
                    { new Guid("0d92871d-04d8-4415-84e7-0225ada944a3"), new DateTime(2023, 6, 21, 4, 53, 5, 323, DateTimeKind.Utc).AddTicks(9105), "megumin@konosuba.gg", "megumin", "Megumin", "$2a$11$pVUi8bKl9qWBypS.44AqcuD321So2wbrd7ZtCJBYypzPUpDGrIdiW", 1, "megumin" },
                    { new Guid("a3789e2f-0747-408b-9720-a32eced2e7e1"), new DateTime(2023, 6, 21, 4, 53, 7, 31, DateTimeKind.Utc).AddTicks(5104), "darkness@konosuba.gg", "darkness", "Darkness", "$2a$11$1O.Hri2fto9kz6NwHCf8NuJPWRyl4jluu9ChWgw8Sz1/3RJLBsgcC", 1, "darkness" },
                    { new Guid("a7e4cd59-50f6-4db1-b0cd-98b95db819fb"), new DateTime(2023, 6, 21, 4, 53, 4, 401, DateTimeKind.Utc).AddTicks(1037), "aqua@konosuba.gg", "aqua", "Aqua", "$2a$11$EU01sSJdc4.EDeaC7bU4xeONlQgAHUQ1vvfL8AVvJKJyOGMzFC2CW", 1, "aqua" },
                    { new Guid("e1f0c96b-98fb-4401-b760-ab7a5ef43ff2"), new DateTime(2023, 6, 21, 4, 53, 3, 56, DateTimeKind.Utc).AddTicks(4477), "kazuma@konosuba.gg", "kazuma", "Kazuma", "$2a$11$CWse9HsKXAy.BEHtTsSk7eWFzyZc7.haKtBp96F/zvrG01Lddqbsu", 1, "kazuma" }
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
