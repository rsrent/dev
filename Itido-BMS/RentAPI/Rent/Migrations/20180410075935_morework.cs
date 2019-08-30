using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class morework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    Log = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MoreWork",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualCompletedTime = table.Column<DateTime>(nullable: false),
                    CleaningPlanID = table.Column<int>(nullable: false),
                    CreatedByUserID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExpectedCompletedTime = table.Column<DateTime>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoreWork", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationLog");

            migrationBuilder.DropTable(
                name: "MoreWork");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "User");
        }
    }
}
