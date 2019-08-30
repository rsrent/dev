using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class HolidaysAndPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CountryCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => new { x.Name, x.CountryCode });
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    SendTime = table.Column<DateTime>(nullable: false),
                    UserRole = table.Column<string>(nullable: true),
                    LocationID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Posts_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkHoliday",
                columns: table => new
                {
                    HolidayName = table.Column<string>(nullable: false),
                    HolidayCountryCode = table.Column<string>(nullable: false),
                    WorkContractID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHoliday", x => new { x.HolidayName, x.HolidayCountryCode, x.WorkContractID });
                    table.ForeignKey(
                        name: "FK_WorkHoliday_WorkContract_WorkContractID",
                        column: x => x.WorkContractID,
                        principalTable: "WorkContract",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkHoliday_Holiday_HolidayName_HolidayCountryCode",
                        columns: x => new { x.HolidayName, x.HolidayCountryCode },
                        principalTable: "Holiday",
                        principalColumns: new[] { "Name", "CountryCode" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CustomerID",
                table: "Posts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationID",
                table: "Posts",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHoliday_WorkContractID",
                table: "WorkHoliday",
                column: "WorkContractID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "WorkHoliday");

            migrationBuilder.DropTable(
                name: "Holiday");
        }
    }
}
