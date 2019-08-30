using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class addedQualityReportCounterToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfQualityReportsSinceUpdate",
                table: "Location",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfQualityReportsSinceUpdate",
                table: "Location");
        }
    }
}
