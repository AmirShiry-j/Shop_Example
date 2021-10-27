using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop_Example.DataLayer.Migrations
{
    public partial class Add_And_Update_Some_Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageStars",
                table: "Stars",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "CAST((([Beauty]+[Ability]+[EasyUse]+[Innovation]+[QualityBuild]+[Affordable])/6.0) AS decimal(5, 2))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageStars",
                table: "Stars");
        }
    }
}
