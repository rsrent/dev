using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class noti_got_title_and_body : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Notis",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notis",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Notis");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notis");
        }
    }
}
