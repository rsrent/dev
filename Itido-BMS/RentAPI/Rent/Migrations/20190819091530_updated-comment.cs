using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updatedcomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Comment",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Comment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Comment",
                newName: "Text");
        }
    }
}
