using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class UpdateToLocationHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPercentage",
                table: "LocationEconomy");

            migrationBuilder.AddColumn<bool>(
                name: "DifferentWeeks",
                table: "LocationHour",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "LocationEconomy",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifferentWeeks",
                table: "LocationHour");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "LocationEconomy",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ProductPercentage",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
