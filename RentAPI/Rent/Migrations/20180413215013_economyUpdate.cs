using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class economyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysPerWeek",
                table: "LocationEconomy");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "LocationEconomy");

            migrationBuilder.RenameColumn(
                name: "WeeklyExpectedHours",
                table: "LocationEconomy",
                newName: "PriceWindowCleaning");

            migrationBuilder.RenameColumn(
                name: "PricePerHour",
                table: "LocationEconomy",
                newName: "PriceRegularCleaning");

            migrationBuilder.AddColumn<int>(
                name: "PricePerHourCategory",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerHourCategory",
                table: "LocationEconomy");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "LocationEconomy");

            migrationBuilder.RenameColumn(
                name: "PriceWindowCleaning",
                table: "LocationEconomy",
                newName: "WeeklyExpectedHours");

            migrationBuilder.RenameColumn(
                name: "PriceRegularCleaning",
                table: "LocationEconomy",
                newName: "PricePerHour");

            migrationBuilder.AddColumn<int>(
                name: "DaysPerWeek",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "MonthlyPrice",
                table: "LocationEconomy",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
