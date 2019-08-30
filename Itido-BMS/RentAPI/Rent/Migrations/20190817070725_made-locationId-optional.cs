using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class madelocationIdoptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Location_LocationId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityReport_Location_LocationID",
                table: "QualityReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "WorkContract",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Work",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "QualityReport",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "News",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "LocationLog",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Location_LocationId",
                table: "News",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityReport_Location_LocationID",
                table: "QualityReport",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Location_LocationId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityReport_Location_LocationID",
                table: "QualityReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "WorkContract",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Work",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "QualityReport",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "News",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "LocationLog",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLog_Location_LocationID",
                table: "LocationLog",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Location_LocationId",
                table: "News",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityReport_Location_LocationID",
                table: "QualityReport",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
