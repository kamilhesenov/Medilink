using Microsoft.EntityFrameworkCore.Migrations;

namespace Medilink_Final_Project.Migrations
{
    public partial class CountPropertyAddShopClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Shops");
        }
    }
}
