using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class removeRequestIdInvi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Requests_RequestID",
                table: "Invitation");

            migrationBuilder.DropIndex(
                name: "IX_Invitation_RequestID",
                table: "Invitation");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Invitation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Invitation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_RequestID",
                table: "Invitation",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Requests_RequestID",
                table: "Invitation",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
