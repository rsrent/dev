using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projectitemaccessrole_step2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAllPermissions",
                table: "ProjectRole",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAllPermissions",
                table: "ProjectRole");
        }
    }
}
