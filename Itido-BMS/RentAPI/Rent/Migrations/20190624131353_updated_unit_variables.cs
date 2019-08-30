using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updated_unit_variables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Access",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EconomyAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HourAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskAccess",
                table: "Units",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DocumentAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "EconomyAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "HourAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "LogAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ReportAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ShiftAccess",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "TaskAccess",
                table: "Units");
        }
    }
}
