using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop_Example.DataLayer.Migrations
{
    public partial class Add_Star_Tabls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EasyUse = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    Beauty = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    QualityBuild = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    Affordable = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    Innovation = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    Ability = table.Column<byte>(type: "tinyint", maxLength: 5, nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stars_CommentId",
                table: "Stars",
                column: "CommentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stars");
        }
    }
}
