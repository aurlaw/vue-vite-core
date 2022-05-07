using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueViteCore.Business.Persistence.Migrations
{
    public partial class AddTodoDoneField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "TodoItems");
        }
    }
}
