using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class new_location_documents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HourText",
                table: "LocationUser",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CleaningFolderID",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FanCoilFolderID",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodicFolderID",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WindowFolderID",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_CleaningFolderID",
                table: "Location",
                column: "CleaningFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_FanCoilFolderID",
                table: "Location",
                column: "FanCoilFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_PeriodicFolderID",
                table: "Location",
                column: "PeriodicFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_WindowFolderID",
                table: "Location",
                column: "WindowFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_CleaningFolderID",
                table: "Location",
                column: "CleaningFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_FanCoilFolderID",
                table: "Location",
                column: "FanCoilFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_PeriodicFolderID",
                table: "Location",
                column: "PeriodicFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_WindowFolderID",
                table: "Location",
                column: "WindowFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_CleaningFolderID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_FanCoilFolderID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_PeriodicFolderID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_WindowFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_CleaningFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_FanCoilFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_PeriodicFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_WindowFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "HourText",
                table: "LocationUser");

            migrationBuilder.DropColumn(
                name: "CleaningFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "FanCoilFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "PeriodicFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "WindowFolderID",
                table: "Location");
        }
    }
}
