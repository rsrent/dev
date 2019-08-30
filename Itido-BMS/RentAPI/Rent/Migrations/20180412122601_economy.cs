using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class economy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_DocumentFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateDocumentFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location");

            migrationBuilder.RenameColumn(
                name: "DocumentFolderID",
                table: "Location",
                newName: "GeneralFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Location_DocumentFolderID",
                table: "Location",
                newName: "IX_Location_GeneralFolderID");

            migrationBuilder.RenameColumn(
                name: "PrivateDocumentFolderID",
                table: "Customer",
                newName: "PrivateFolderID");

            migrationBuilder.RenameColumn(
                name: "DocumentFolderID",
                table: "Customer",
                newName: "GeneralFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_PrivateDocumentFolderID",
                table: "Customer",
                newName: "IX_Customer_PrivateFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_DocumentFolderID",
                table: "Customer",
                newName: "IX_Customer_GeneralFolderID");

            migrationBuilder.AddColumn<int>(
                name: "CleaningplanFolderID",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasParentPermissions",
                table: "DocumentFolder",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasStandardFolders",
                table: "Customer",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagementFolderID",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FolderPermission",
                columns: table => new
                {
                    FolderID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderPermission", x => new { x.FolderID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_FolderPermission_DocumentFolder_FolderID",
                        column: x => x.FolderID,
                        principalTable: "DocumentFolder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationEconomy",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DaysPerWeek = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    MonthlyPrice = table.Column<float>(nullable: false),
                    PricePerHour = table.Column<float>(nullable: false),
                    ProductPercentage = table.Column<float>(nullable: false),
                    WeeklyExpectedHours = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationEconomy", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_CleaningplanFolderID",
                table: "Location",
                column: "CleaningplanFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ManagementFolderID",
                table: "Customer",
                column: "ManagementFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_GeneralFolderID",
                table: "Customer",
                column: "GeneralFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_ManagementFolderID",
                table: "Customer",
                column: "ManagementFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateFolderID",
                table: "Customer",
                column: "PrivateFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_CleaningplanFolderID",
                table: "Location",
                column: "CleaningplanFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_GeneralFolderID",
                table: "Location",
                column: "GeneralFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_GeneralFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_ManagementFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_CleaningplanFolderID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_GeneralFolderID",
                table: "Location");

            migrationBuilder.DropTable(
                name: "FolderPermission");

            migrationBuilder.DropTable(
                name: "LocationEconomy");

            migrationBuilder.DropIndex(
                name: "IX_Location_CleaningplanFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ManagementFolderID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CleaningplanFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "HasParentPermissions",
                table: "DocumentFolder");

            migrationBuilder.DropColumn(
                name: "HasStandardFolders",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ManagementFolderID",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "GeneralFolderID",
                table: "Location",
                newName: "DocumentFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Location_GeneralFolderID",
                table: "Location",
                newName: "IX_Location_DocumentFolderID");

            migrationBuilder.RenameColumn(
                name: "PrivateFolderID",
                table: "Customer",
                newName: "PrivateDocumentFolderID");

            migrationBuilder.RenameColumn(
                name: "GeneralFolderID",
                table: "Customer",
                newName: "DocumentFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_PrivateFolderID",
                table: "Customer",
                newName: "IX_Customer_PrivateDocumentFolderID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_GeneralFolderID",
                table: "Customer",
                newName: "IX_Customer_DocumentFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_DocumentFolderID",
                table: "Customer",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateDocumentFolderID",
                table: "Customer",
                column: "PrivateDocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
