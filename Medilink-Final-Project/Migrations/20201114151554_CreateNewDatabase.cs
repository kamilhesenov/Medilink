using Microsoft.EntityFrameworkCore.Migrations;

namespace Medilink_Final_Project.Migrations
{
    public partial class CreateNewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeSpecialitySucces",
                table: "HomeSpecialitySucces");

            migrationBuilder.RenameTable(
                name: "HomeSpecialitySucces",
                newName: "homeSpecialitySucces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_homeSpecialitySucces",
                table: "homeSpecialitySucces",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_homeSpecialitySucces",
                table: "homeSpecialitySucces");

            migrationBuilder.RenameTable(
                name: "homeSpecialitySucces",
                newName: "HomeSpecialitySucces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeSpecialitySucces",
                table: "HomeSpecialitySucces",
                column: "Id");
        }
    }
}
