using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class smul_changi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatePasswordsAtCronInterval",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "UpdatePasswordsAtSecondsInterval",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatePasswordsAtSecondsInterval",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "UpdatePasswordsAtCronInterval",
                table: "Customer",
                nullable: true);
        }
    }
}
