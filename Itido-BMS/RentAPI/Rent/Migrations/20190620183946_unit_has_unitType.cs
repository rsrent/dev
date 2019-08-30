using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class unit_has_unitType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitType",
                table: "Units",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "Units");
        }
    }
}
