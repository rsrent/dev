using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class IsEvenWeekToByte_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay",
                columns: new[] { "WorkContractID", "DayOfWeek", "IsEvenWeek" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay",
                columns: new[] { "WorkContractID", "DayOfWeek" });
        }
    }
}
