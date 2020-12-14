using Microsoft.EntityFrameworkCore.Migrations;

namespace Medilink_Final_Project.Migrations
{
    public partial class ShopClassPriceTypeMoneyDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Shops",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Shops",
                type: "money",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
