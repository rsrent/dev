using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class smallprojectchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FirestoreConversation_ProjectItemID",
                table: "FirestoreConversation");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder");

            migrationBuilder.CreateIndex(
                name: "IX_FirestoreConversation_ProjectItemID",
                table: "FirestoreConversation",
                column: "ProjectItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder",
                column: "ProjectItemID",
                unique: true,
                filter: "[ProjectItemID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FirestoreConversation_ProjectItemID",
                table: "FirestoreConversation");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder");

            migrationBuilder.CreateIndex(
                name: "IX_FirestoreConversation_ProjectItemID",
                table: "FirestoreConversation",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder",
                column: "ProjectItemID");
        }
    }
}
