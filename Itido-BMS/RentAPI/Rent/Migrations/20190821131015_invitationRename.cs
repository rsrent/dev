using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class invitationRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Requests_RequestID",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_User_UserID",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Work_WorkID",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitation");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_WorkID",
                table: "Invitation",
                newName: "IX_Invitation_WorkID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_UserID",
                table: "Invitation",
                newName: "IX_Invitation_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_RequestID",
                table: "Invitation",
                newName: "IX_Invitation_RequestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Requests_RequestID",
                table: "Invitation",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_User_UserID",
                table: "Invitation",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Work_WorkID",
                table: "Invitation",
                column: "WorkID",
                principalTable: "Work",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Requests_RequestID",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_User_UserID",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Work_WorkID",
                table: "Invitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation");

            migrationBuilder.RenameTable(
                name: "Invitation",
                newName: "Invitations");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_WorkID",
                table: "Invitations",
                newName: "IX_Invitations_WorkID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_UserID",
                table: "Invitations",
                newName: "IX_Invitations_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_RequestID",
                table: "Invitations",
                newName: "IX_Invitations_RequestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Requests_RequestID",
                table: "Invitations",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_User_UserID",
                table: "Invitations",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Work_WorkID",
                table: "Invitations",
                column: "WorkID",
                principalTable: "Work",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
