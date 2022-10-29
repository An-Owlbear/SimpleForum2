using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleForum.Migrations
{
    public partial class Added_ProfileComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileComment",
                columns: table => new
                {
                    ProfileCommentId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RecipientProfileId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileComment", x => x.ProfileCommentId);
                    table.ForeignKey(
                        name: "FK_ProfileComment_User_RecipientProfileId",
                        column: x => x.RecipientProfileId,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileComment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileComment_RecipientProfileId",
                table: "ProfileComment",
                column: "RecipientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileComment_UserId",
                table: "ProfileComment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileComment");
        }
    }
}
