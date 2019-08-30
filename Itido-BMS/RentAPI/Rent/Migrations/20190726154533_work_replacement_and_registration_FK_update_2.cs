using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class work_replacement_and_registration_FK_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkRegistration");

            migrationBuilder.DropTable(
                name: "WorkReplacement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkRegistration",
                columns: table => new
                {
                    WorkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApprovalState = table.Column<int>(nullable: false),
                    BreakMins = table.Column<short>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EndTimeMins = table.Column<short>(nullable: false),
                    StartTimeMins = table.Column<short>(nullable: false)
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
                    AbsenceID = table.Column<int>(nullable: false),
                    ContractID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkReplacement", x => x.WorkID);
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_AbsenceID",
                table: "WorkReplacement",
                column: "AbsenceID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_ContractID",
                table: "WorkReplacement",
                column: "ContractID");
        }
    }
}
