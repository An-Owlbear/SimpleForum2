using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleForum.Migrations
{
    public partial class Moved_thread_content_back_to_thread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpeningPost",
                table: "Reply");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Thread",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Thread");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpeningPost",
                table: "Reply",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
