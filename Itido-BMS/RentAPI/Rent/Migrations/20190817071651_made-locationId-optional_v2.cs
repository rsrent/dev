using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class madelocationIdoptional_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_LocationID",
                table: "CleaningTask");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "CleaningTask",
                nullable: true,
                oldClrType: typeof(int));

            /*
            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Location_LocationID",
                table: "CleaningTask",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Location_LocationID",
                table: "CleaningTask");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "CleaningTask",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Location_LocationID",
                table: "CleaningTask",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
