using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleForum.Migrations
{
    public partial class AddedOpeningPostFieldToReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpeningPost",
                table: "Reply",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpeningPost",
                table: "Reply");
        }
    }
}
