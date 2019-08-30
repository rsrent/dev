using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class AddedPrivateDocumentFolderToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrivateDocumentFolderID",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PrivateDocumentFolderID",
                table: "Customer",
                column: "PrivateDocumentFolderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateDocumentFolderID",
                table: "Customer",
                column: "PrivateDocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateDocumentFolderID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_PrivateDocumentFolderID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PrivateDocumentFolderID",
                table: "Customer");
        }
    }
}
