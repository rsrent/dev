using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class WorkReplacementPKAndNotUserID4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkRegistration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    StartTimeMins = table.Column<short>(nullable: false),
                    EndTimeMins = table.Column<short>(nullable: false),
                    BreakMins = table.Column<short>(nullable: false),
                    ApprovalState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRegistration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkRegistration_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkReplacement",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkID = table.Column<int>(nullable: false),
                    ContractID = table.Column<int>(nullable: true),
                    AbsenceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkReplacement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkReplacement_Absence_AbsenceID",
                        column: x => x.AbsenceID,
                        principalTable: "Absence",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkReplacement_Contract_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contract",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkReplacement_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkRegistration_WorkID",
                table: "WorkRegistration",
                column: "WorkID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_AbsenceID",
                table: "WorkReplacement",
                column: "AbsenceID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_ContractID",
                table: "WorkReplacement",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_WorkID",
                table: "WorkReplacement",
                column: "WorkID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkRegistration");

            migrationBuilder.DropTable(
                name: "WorkReplacement");
        }
    }
}
