using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class registrationWithRequestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "WorkRegistration",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkRegistration_RequestID",
                table: "WorkRegistration",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRegistration_Requests_RequestID",
                table: "WorkRegistration",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkRegistration_Requests_RequestID",
                table: "WorkRegistration");

            migrationBuilder.DropIndex(
                name: "IX_WorkRegistration_RequestID",
                table: "WorkRegistration");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "WorkRegistration");
        }
    }
}
