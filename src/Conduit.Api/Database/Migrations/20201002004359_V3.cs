using Microsoft.EntityFrameworkCore.Migrations;

namespace Conduit.Api.Database.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleComment",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ArticleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComment_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleComment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_ArticleId",
                table: "ArticleComment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_UserId",
                table: "ArticleComment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleComment");
        }
    }
}
