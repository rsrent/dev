using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class absenceondeletesetnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbsenceReason",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    RolesToCreate = table.Column<string>(nullable: true),
                    RolesToAccept = table.Column<string>(nullable: true),
                    CanUserCreate = table.Column<bool>(nullable: false),
                    CanUserRequest = table.Column<bool>(nullable: false),
                    CanManagerCreate = table.Column<bool>(nullable: false),
                    CanManagerRequest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceReason", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Absence",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    ApprovalState = table.Column<int>(nullable: false),
                    CreatorID = table.Column<int>(nullable: true),
                    AbsenceReasonID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Absence_AbsenceReason_AbsenceReasonID",
                        column: x => x.AbsenceReasonID,
                        principalTable: "AbsenceReason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Absence_User_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Absence_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AbsenceReasonID",
                table: "Absence",
                column: "AbsenceReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_CreatorID",
                table: "Absence",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_UserID",
                table: "Absence",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absence");

            migrationBuilder.DropTable(
                name: "AbsenceReason");
        }
    }
}
