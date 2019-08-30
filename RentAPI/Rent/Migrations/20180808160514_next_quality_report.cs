using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class next_quality_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextReport",
                table: "QualityReport",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextReport",
                table: "QualityReport");
        }
    }
}
