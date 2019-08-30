using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class location_first_quality_report_time : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstQualityReportTime",
                table: "Location",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE Location SET Location.FirstQualityReportTime = (SELECT C1.Time FROM QualityReport C1 WHERE C1.ID IN (SELECT MIN(C2.ID) FROM QualityReport C2 WHERE C2.LocationID = Location.ID))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstQualityReportTime",
                table: "Location");
        }
    }
}
