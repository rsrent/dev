using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class work_replacement_and_registration_FK_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_WorkRegistration_WorkRegistrationID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_WorkReplacement_WorkReplacementID",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_WorkRegistrationID",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_WorkReplacementID",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "WorkRegistrationID",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "WorkReplacementID",
                table: "Work");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_AbsenceID",
                table: "WorkReplacement",
                column: "AbsenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkReplacement_Absence_AbsenceID",
                table: "WorkReplacement",
                column: "AbsenceID",
                principalTable: "Absence",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkReplacement_Absence_AbsenceID",
                table: "WorkReplacement");

            migrationBuilder.DropIndex(
                name: "IX_WorkReplacement_AbsenceID",
                table: "WorkReplacement");

            migrationBuilder.AddColumn<int>(
                name: "WorkRegistrationID",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkReplacementID",
                table: "Work",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkRegistrationID",
                table: "Work",
                column: "WorkRegistrationID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkReplacementID",
                table: "Work",
                column: "WorkReplacementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_WorkRegistration_WorkRegistrationID",
                table: "Work",
                column: "WorkRegistrationID",
                principalTable: "WorkRegistration",
                principalColumn: "WorkID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_WorkReplacement_WorkReplacementID",
                table: "Work",
                column: "WorkReplacementID",
                principalTable: "WorkReplacement",
                principalColumn: "WorkID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
