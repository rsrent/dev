using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class documentfolderhasrootknowledge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RootDocumentFolderID",
                table: "DocumentFolder",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_RootDocumentFolderID",
                table: "DocumentFolder",
                column: "RootDocumentFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFolder_DocumentFolder_RootDocumentFolderID",
                table: "DocumentFolder",
                column: "RootDocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFolder_DocumentFolder_RootDocumentFolderID",
                table: "DocumentFolder");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_RootDocumentFolderID",
                table: "DocumentFolder");

            migrationBuilder.DropColumn(
                name: "RootDocumentFolderID",
                table: "DocumentFolder");
        }
    }
}
