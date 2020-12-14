using Microsoft.EntityFrameworkCore.Migrations;

namespace Medilink_Final_Project.Migrations
{
    public partial class ShopCheckoutPewodTableDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopCheckouts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopCheckouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckoutId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCheckouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopCheckouts_Checkouts_CheckoutId",
                        column: x => x.CheckoutId,
                        principalTable: "Checkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopCheckouts_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopCheckouts_CheckoutId",
                table: "ShopCheckouts",
                column: "CheckoutId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCheckouts_ShopId",
                table: "ShopCheckouts",
                column: "ShopId");
        }
    }
}
