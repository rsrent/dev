using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class location_has_unit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitID",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_UnitID",
                table: "Location",
                column: "UnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Units_UnitID",
                table: "Location",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Units_UnitID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_UnitID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UnitID",
                table: "Location");
        }
    }
}
