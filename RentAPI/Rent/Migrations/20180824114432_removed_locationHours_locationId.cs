using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class removed_locationHours_locationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "LocationHour");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "LocationHour",
                nullable: false,
                defaultValue: 0);
        }
    }
}
