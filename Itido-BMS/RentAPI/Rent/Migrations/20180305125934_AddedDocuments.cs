using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class AddedDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "QualityReport",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "DocumentFolderID",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Relevance",
                table: "Floor",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Relevance",
                table: "Area",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocumentFolder",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentDocumentFolderID = table.Column<int>(nullable: true),
                    Standard = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFolder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContainerType = table.Column<int>(nullable: false),
                    DocumentLocation = table.Column<string>(nullable: true),
                    RootDocumentFolderID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentItem", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location");

            migrationBuilder.DropTable(
                name: "DocumentFolder");

            migrationBuilder.DropTable(
                name: "DocumentItem");

            migrationBuilder.DropIndex(
                name: "IX_Location_DocumentFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "DocumentFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Relevance",
                table: "Floor");

            migrationBuilder.DropColumn(
                name: "Relevance",
                table: "Area");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "QualityReport",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
