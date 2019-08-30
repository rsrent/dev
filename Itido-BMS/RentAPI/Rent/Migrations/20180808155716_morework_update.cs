using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class morework_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Hours",
                table: "MoreWork",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoreWork_LocationID",
                table: "MoreWork",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationLog_LocationID",
                table: "LocationLog",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationLog_UserID",
                table: "LocationLog",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLog_User_UserID",
                table: "LocationLog",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoreWork_Location_LocationID",
                table: "MoreWork",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationLog_User_UserID",
                table: "LocationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_MoreWork_Location_LocationID",
                table: "MoreWork");

            migrationBuilder.DropIndex(
                name: "IX_MoreWork_LocationID",
                table: "MoreWork");

            migrationBuilder.DropIndex(
                name: "IX_LocationLog_LocationID",
                table: "LocationLog");

            migrationBuilder.DropIndex(
                name: "IX_LocationLog_UserID",
                table: "LocationLog");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "MoreWork");
        }
    }
}
