using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueViteCore.Business.Persistence.Migrations
{
    public partial class AddSubEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubmissionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueOne = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ValueTwo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ValueThree = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionEntries");
        }
    }
}
