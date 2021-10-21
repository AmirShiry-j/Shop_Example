using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop_Example.DataLayer.Migrations
{
    public partial class Update_Config_ProductInCategory_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInCategory_Categories_CategoryId",
                table: "ProductInCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInCategory_Categories_CategoryId",
                table: "ProductInCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInCategory_Categories_CategoryId",
                table: "ProductInCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInCategory_Categories_CategoryId",
                table: "ProductInCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
