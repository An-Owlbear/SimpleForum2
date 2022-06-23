using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleForum.Migrations
{
    public partial class AddedNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Thread_UserId",
                table: "Thread",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thread_User_UserId",
                table: "Thread",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thread_User_UserId",
                table: "Thread");

            migrationBuilder.DropIndex(
                name: "IX_Thread_UserId",
                table: "Thread");
        }
    }
}
