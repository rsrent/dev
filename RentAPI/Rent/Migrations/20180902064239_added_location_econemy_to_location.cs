using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class added_location_econemy_to_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "LocationEconomyID",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE Location SET Location.LocationEconomyID = (SELECT LocationEconomy.ID FROM LocationEconomy WHERE LocationEconomy.LocationID = Location.ID)");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "LocationEconomy");

            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationEconomyID",
                table: "Location",
                column: "LocationEconomyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationEconomy_LocationEconomyID",
                table: "Location",
                column: "LocationEconomyID",
                principalTable: "LocationEconomy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationEconomy_LocationEconomyID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_LocationEconomyID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationEconomyID",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: 0);
        }
    }
}
