using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class IsEvenWeekToByte_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay");

            migrationBuilder.AlterColumn<byte>(
                name: "IsEvenWeek",
                table: "WorkDay",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay",
                columns: new[] { "WorkContractID", "DayOfWeek" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEvenWeek",
                table: "WorkDay",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDay",
                table: "WorkDay",
                columns: new[] { "WorkContractID", "DayOfWeek", "IsEvenWeek" });
        }
    }
}
