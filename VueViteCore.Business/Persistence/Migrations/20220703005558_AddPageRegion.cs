using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueViteCore.Business.Persistence.Migrations
{
    public partial class AddPageRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Page = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageRegions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageRegions_Page",
                table: "PageRegions",
                column: "Page");

            migrationBuilder.CreateIndex(
                name: "IX_PageRegions_Page_Region",
                table: "PageRegions",
                columns: new[] { "Page", "Region" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageRegions");
        }
    }
}
