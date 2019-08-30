using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class request_and_accidentReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovalState",
                table: "Absence",
                newName: "RequestID");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApprovalState = table.Column<int>(nullable: false),
                    RespondDateTime = table.Column<DateTime>(nullable: true),
                    CreatorID = table.Column<int>(nullable: true),
                    SubjectID = table.Column<int>(nullable: true),
                    ResponderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Requests_User_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_ResponderID",
                        column: x => x.ResponderID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccidentReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccidentReportType = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActionTaken = table.Column<string>(nullable: true),
                    AbsenceDurationDays = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: true),
                    CreatorID = table.Column<int>(nullable: true),
                    RequestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccidentReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccidentReports_User_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccidentReports_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccidentReports_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_RequestID",
                table: "Absence",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AccidentReports_CreatorID",
                table: "AccidentReports",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_AccidentReports_RequestID",
                table: "AccidentReports",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AccidentReports_UserID",
                table: "AccidentReports",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatorID",
                table: "Requests",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ResponderID",
                table: "Requests",
                column: "ResponderID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SubjectID",
                table: "Requests",
                column: "SubjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Requests_RequestID",
                table: "Absence",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Requests_RequestID",
                table: "Absence");

            migrationBuilder.DropTable(
                name: "AccidentReports");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Absence_RequestID",
                table: "Absence");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "Absence",
                newName: "ApprovalState");
        }
    }
}
