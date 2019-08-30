using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updated_clients_step_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Units_UnitID",
                table: "Location");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "DivisionUsers");

            migrationBuilder.DropTable(
                name: "UnitUsers");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Location_UnitID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UnitID",
                table: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitID",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DivisionUsers",
                columns: table => new
                {
                    DivisionID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionUsers", x => new { x.DivisionID, x.UserID });
                    table.ForeignKey(
                        name: "FK_DivisionUsers_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DivisionUsers_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Access = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionID = table.Column<int>(nullable: true),
                    DocumentAccess = table.Column<string>(maxLength: 10, nullable: true),
                    EconomyAccess = table.Column<string>(maxLength: 10, nullable: true),
                    HourAccess = table.Column<string>(maxLength: 10, nullable: true),
                    LogAccess = table.Column<string>(maxLength: 10, nullable: true),
                    ParentID = table.Column<int>(nullable: true),
                    ReportAccess = table.Column<string>(maxLength: 10, nullable: true),
                    ShiftAccess = table.Column<string>(maxLength: 10, nullable: true),
                    TaskAccess = table.Column<string>(maxLength: 10, nullable: true),
                    UnitType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Units_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Units_Units_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Units",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    Disabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UnitID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitUsers",
                columns: table => new
                {
                    UnitID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitUsers", x => new { x.UnitID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UnitUsers_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitUsers_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_UnitID",
                table: "Location",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_Client_UnitID",
                table: "Client",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionUsers_UserID",
                table: "DivisionUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DivisionID",
                table: "Units",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_ParentID",
                table: "Units",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitUsers_UserID",
                table: "UnitUsers",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Units_UnitID",
                table: "Location",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
