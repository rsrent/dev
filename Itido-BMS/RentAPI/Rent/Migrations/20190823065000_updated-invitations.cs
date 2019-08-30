using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updatedinvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitation");

            migrationBuilder.CreateTable(
                name: "WorkInvitation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractID = table.Column<int>(nullable: false),
                    WorkID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkInvitation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkInvitation_Contract_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contract",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkInvitation_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkInvitation_ContractID",
                table: "WorkInvitation",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkInvitation_WorkID",
                table: "WorkInvitation",
                column: "WorkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkInvitation");

            migrationBuilder.CreateTable(
                name: "Invitation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    WorkID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Invitation_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitation_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_UserID",
                table: "Invitation",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_WorkID",
                table: "Invitation",
                column: "WorkID");
        }
    }
}
