using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppNary.Data.Migrations
{
    public partial class can_show_recipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanShow",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanShow",
                table: "Recipes");
        }
    }
}
