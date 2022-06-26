using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleForum.Migrations
{
    public partial class AddedForumModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForumId",
                table: "Thread",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Forum",
                columns: table => new
                {
                    ForumId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.ForumId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Thread_ForumId",
                table: "Thread",
                column: "ForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thread_Forum_ForumId",
                table: "Thread",
                column: "ForumId",
                principalTable: "Forum",
                principalColumn: "ForumId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thread_Forum_ForumId",
                table: "Thread");

            migrationBuilder.DropTable(
                name: "Forum");

            migrationBuilder.DropIndex(
                name: "IX_Thread_ForumId",
                table: "Thread");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "Thread");
        }
    }
}
