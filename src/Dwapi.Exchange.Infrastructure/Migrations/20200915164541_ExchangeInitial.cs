using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Exchange.Infrastructure.Migrations
{
    public partial class ExchangeInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Purpose = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtractRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SqlScript = table.Column<string>(nullable: true),
                    RecordCount = table.Column<long>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Refreshed = table.Column<DateTime>(nullable: false),
                    RegistryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtractRequests_Registries_RegistryId",
                        column: x => x.RegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtractRequests_RegistryId",
                table: "ExtractRequests",
                column: "RegistryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractRequests");

            migrationBuilder.DropTable(
                name: "Registries");
        }
    }
}
