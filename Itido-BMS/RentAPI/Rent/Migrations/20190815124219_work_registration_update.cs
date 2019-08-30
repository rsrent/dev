using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class work_registration_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovalState",
                table: "WorkRegistration",
                newName: "UserID");

            migrationBuilder.AlterColumn<short>(
                name: "StartTimeMins",
                table: "WorkRegistration",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "EndTimeMins",
                table: "WorkRegistration",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "BreakMins",
                table: "WorkRegistration",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.CreateIndex(
                name: "IX_WorkRegistration_UserID",
                table: "WorkRegistration",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRegistration_User_UserID",
                table: "WorkRegistration",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkRegistration_User_UserID",
                table: "WorkRegistration");

            migrationBuilder.DropIndex(
                name: "IX_WorkRegistration_UserID",
                table: "WorkRegistration");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "WorkRegistration",
                newName: "ApprovalState");

            migrationBuilder.AlterColumn<short>(
                name: "StartTimeMins",
                table: "WorkRegistration",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "EndTimeMins",
                table: "WorkRegistration",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "BreakMins",
                table: "WorkRegistration",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
