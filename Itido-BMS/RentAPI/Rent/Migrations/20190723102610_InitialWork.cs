using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class InitialWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkContract",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractID = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    ContractAgreementID = table.Column<int>(nullable: true),
                    ContractUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkContract", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkContract_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkContract_Contract_ContractAgreementID_ContractUserID",
                        columns: x => new { x.ContractAgreementID, x.ContractUserID },
                        principalTable: "Contract",
                        principalColumns: new[] { "AgreementID", "UserID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkRegistration",
                columns: table => new
                {
                    WorkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    StartTimeMins = table.Column<short>(nullable: false),
                    EndTimeMins = table.Column<short>(nullable: false),
                    BreakMins = table.Column<short>(nullable: false),
                    ApprovalState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRegistration", x => x.WorkID);
                });

            migrationBuilder.CreateTable(
                name: "WorkReplacement",
                columns: table => new
                {
                    WorkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractID = table.Column<int>(nullable: true),
                    AbsenceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkReplacement", x => x.WorkID);
                });

            migrationBuilder.CreateTable(
                name: "WorkDay",
                columns: table => new
                {
                    WorkContractID = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    IsEvenWeek = table.Column<bool>(nullable: false),
                    StartTimeMins = table.Column<short>(nullable: false),
                    EndTimeMins = table.Column<short>(nullable: false),
                    BreakMins = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDay", x => new { x.WorkContractID, x.DayOfWeek, x.IsEvenWeek });
                    table.ForeignKey(
                        name: "FK_WorkDay_WorkContract_WorkContractID",
                        column: x => x.WorkContractID,
                        principalTable: "WorkContract",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractID = table.Column<int>(nullable: true),
                    WorkContractID = table.Column<int>(nullable: true),
                    LocationID = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    StartTimeMins = table.Column<short>(nullable: false),
                    EndTimeMins = table.Column<short>(nullable: false),
                    BreakMins = table.Column<short>(nullable: false),
                    WorkReplacementID = table.Column<int>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: false),
                    WorkRegistrationID = table.Column<int>(nullable: true),
                    ContractAgreementID = table.Column<int>(nullable: true),
                    ContractUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Work_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Work_WorkRegistration_WorkRegistrationID",
                        column: x => x.WorkRegistrationID,
                        principalTable: "WorkRegistration",
                        principalColumn: "WorkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Work_WorkReplacement_WorkReplacementID",
                        column: x => x.WorkReplacementID,
                        principalTable: "WorkReplacement",
                        principalColumn: "WorkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Work_Contract_ContractAgreementID_ContractUserID",
                        columns: x => new { x.ContractAgreementID, x.ContractUserID },
                        principalTable: "Contract",
                        principalColumns: new[] { "AgreementID", "UserID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Work_LocationID",
                table: "Work",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkRegistrationID",
                table: "Work",
                column: "WorkRegistrationID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkReplacementID",
                table: "Work",
                column: "WorkReplacementID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_ContractAgreementID_ContractUserID",
                table: "Work",
                columns: new[] { "ContractAgreementID", "ContractUserID" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_LocationID",
                table: "WorkContract",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_ContractAgreementID_ContractUserID",
                table: "WorkContract",
                columns: new[] { "ContractAgreementID", "ContractUserID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Work");

            migrationBuilder.DropTable(
                name: "WorkDay");

            migrationBuilder.DropTable(
                name: "WorkRegistration");

            migrationBuilder.DropTable(
                name: "WorkReplacement");

            migrationBuilder.DropTable(
                name: "WorkContract");
        }
    }
}
