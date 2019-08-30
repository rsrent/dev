using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class tree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Units_Units_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Units",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    UnitID = table.Column<int>(nullable: false)
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
                name: "IX_Units_ParentID",
                table: "Units",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitUsers_UserID",
                table: "UnitUsers",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitUsers");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
