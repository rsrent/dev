using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class added_lat_lon_to_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Lon",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "Location");
        }
    }
}
