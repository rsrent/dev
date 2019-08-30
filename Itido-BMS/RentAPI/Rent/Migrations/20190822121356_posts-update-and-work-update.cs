using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class postsupdateandworkupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Customer_CustomerID",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Location_LocationID",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_WorkContract_LocationID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_Work_LocationID",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CustomerID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LocationID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "WorkContract");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Posts",
                newName: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ProjectItemID",
                table: "Posts",
                column: "ProjectItemID",
                unique: true,
                filter: "[ProjectItemID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ProjectItem_ProjectItemID",
                table: "Posts",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ProjectItem_ProjectItemID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ProjectItemID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "ProjectItemID",
                table: "Posts",
                newName: "LocationID");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "WorkContract",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Work",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_LocationID",
                table: "WorkContract",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_LocationID",
                table: "Work",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CustomerID",
                table: "Posts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationID",
                table: "Posts",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Customer_CustomerID",
                table: "Posts",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Location_LocationID",
                table: "Posts",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Location_LocationID",
                table: "Work",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_Location_LocationID",
                table: "WorkContract",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
