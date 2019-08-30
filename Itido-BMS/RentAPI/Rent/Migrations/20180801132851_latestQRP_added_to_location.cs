using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class latestQRP_added_to_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LatestQualityReportID",
                table: "Location",
                nullable: true);

            migrationBuilder.Sql("UPDATE Location SET Location.LatestQualityReportID = (SELECT C1.ID FROM QualityReport C1 WHERE C1.Time IN (SELECT MAX(C2.Time) FROM QualityReport C2 WHERE C2.LocationID = Location.ID))");

            migrationBuilder.CreateIndex(
                name: "IX_Location_LatestQualityReportID",
                table: "Location",
                column: "LatestQualityReportID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_QualityReport_LatestQualityReportID",
                table: "Location",
                column: "LatestQualityReportID",
                principalTable: "QualityReport",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_QualityReport_LatestQualityReportID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_LatestQualityReportID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LatestQualityReportID",
                table: "Location");
        }
    }
}
