using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class divisionsNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUser_Division_DivisionID",
                table: "DivisionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUser_User_UserID",
                table: "DivisionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Division_DivisionID",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DivisionUser",
                table: "DivisionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Division",
                table: "Division");

            migrationBuilder.RenameTable(
                name: "DivisionUser",
                newName: "DivisionUsers");

            migrationBuilder.RenameTable(
                name: "Division",
                newName: "Divisions");

            migrationBuilder.RenameIndex(
                name: "IX_DivisionUser_UserID",
                table: "DivisionUsers",
                newName: "IX_DivisionUsers_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DivisionUsers",
                table: "DivisionUsers",
                columns: new[] { "DivisionID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUsers_Divisions_DivisionID",
                table: "DivisionUsers",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUsers_User_UserID",
                table: "DivisionUsers",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Divisions_DivisionID",
                table: "Units",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUsers_Divisions_DivisionID",
                table: "DivisionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUsers_User_UserID",
                table: "DivisionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Divisions_DivisionID",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DivisionUsers",
                table: "DivisionUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions");

            migrationBuilder.RenameTable(
                name: "DivisionUsers",
                newName: "DivisionUser");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "Division");

            migrationBuilder.RenameIndex(
                name: "IX_DivisionUsers_UserID",
                table: "DivisionUser",
                newName: "IX_DivisionUser_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DivisionUser",
                table: "DivisionUser",
                columns: new[] { "DivisionID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Division",
                table: "Division",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUser_Division_DivisionID",
                table: "DivisionUser",
                column: "DivisionID",
                principalTable: "Division",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUser_User_UserID",
                table: "DivisionUser",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Division_DivisionID",
                table: "Units",
                column: "DivisionID",
                principalTable: "Division",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
