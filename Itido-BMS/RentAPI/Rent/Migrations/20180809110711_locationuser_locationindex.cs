using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class locationuser_locationindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LocationUser_LocationID",
                table: "LocationUser",
                column: "LocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationUser_LocationID",
                table: "LocationUser");
        }
    }
}
