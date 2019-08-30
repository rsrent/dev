using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "RolesToAccept",
                table: "AbsenceReason");

            migrationBuilder.DropColumn(
                name: "RolesToCreate",
                table: "AbsenceReason");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Absence",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatorID",
                table: "Absence",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Contract",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RolesToAccept",
                table: "AbsenceReason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RolesToCreate",
                table: "AbsenceReason",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Absence",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CreatorID",
                table: "Absence",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
