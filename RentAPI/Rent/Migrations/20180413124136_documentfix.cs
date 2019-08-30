using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class documentfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_ManagementFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_CleaningplanFolderID",
                table: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "CleaningplanFolderID",
                table: "Location",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrivateFolderID",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagementFolderID",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LocationHour",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChristAscension = table.Column<bool>(nullable: false),
                    ChristmasDay = table.Column<bool>(nullable: false),
                    ChristmasEve = table.Column<bool>(nullable: false),
                    EasterDay = table.Column<bool>(nullable: false),
                    GoodFriday = table.Column<bool>(nullable: false),
                    L_Fri = table.Column<float>(nullable: false),
                    L_Mon = table.Column<float>(nullable: false),
                    L_Sat = table.Column<float>(nullable: false),
                    L_Sun = table.Column<float>(nullable: false),
                    L_Thu = table.Column<float>(nullable: false),
                    L_Tue = table.Column<float>(nullable: false),
                    L_Wed = table.Column<float>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    MaundyThursday = table.Column<bool>(nullable: false),
                    NewyearsDay = table.Column<bool>(nullable: false),
                    NewyearsEve = table.Column<bool>(nullable: false),
                    Palmsunday = table.Column<bool>(nullable: false),
                    PrayerDay = table.Column<bool>(nullable: false),
                    SecondEasterDay = table.Column<bool>(nullable: false),
                    SndChristmasDay = table.Column<bool>(nullable: false),
                    SndPentecost = table.Column<bool>(nullable: false),
                    U_Fri = table.Column<float>(nullable: false),
                    U_Mon = table.Column<float>(nullable: false),
                    U_Sat = table.Column<float>(nullable: false),
                    U_Sun = table.Column<float>(nullable: false),
                    U_Thu = table.Column<float>(nullable: false),
                    U_Tue = table.Column<float>(nullable: false),
                    U_Wed = table.Column<float>(nullable: false),
                    WhitSunday = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationHour", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_ManagementFolderID",
                table: "Customer",
                column: "ManagementFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateFolderID",
                table: "Customer",
                column: "PrivateFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_CleaningplanFolderID",
                table: "Location",
                column: "CleaningplanFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_ManagementFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_PrivateFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_CleaningplanFolderID",
                table: "Location");

            migrationBuilder.DropTable(
                name: "LocationHour");

            migrationBuilder.AlterColumn<int>(
                name: "CleaningplanFolderID",
                table: "Location",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PrivateFolderID",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ManagementFolderID",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(int));

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
        }
    }
}
