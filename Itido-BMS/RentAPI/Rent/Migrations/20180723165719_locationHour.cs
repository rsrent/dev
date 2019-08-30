using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class locationHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "LocationHourID",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationHourID",
                table: "Location",
                column: "LocationHourID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationHour_LocationHourID",
                table: "Location",
                column: "LocationHourID",
                principalTable: "LocationHour",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationHour_LocationHourID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_LocationHourID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationHourID",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Location",
                nullable: false,
                defaultValue: "");
        }
    }
}
