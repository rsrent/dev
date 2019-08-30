using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class locationUserUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "MoreWork",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LocationUser",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationUser_Location_LocationID",
                table: "LocationUser",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationUser_Location_LocationID",
                table: "LocationUser");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LocationUser");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "MoreWork",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
