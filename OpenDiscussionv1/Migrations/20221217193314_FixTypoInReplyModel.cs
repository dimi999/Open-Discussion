using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDiscussionv1.Migrations
{
    public partial class FixTypoInReplyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscussionsId",
                table: "Replies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscussionsId",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
