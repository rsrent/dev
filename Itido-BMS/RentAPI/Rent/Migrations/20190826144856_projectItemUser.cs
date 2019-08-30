using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projectItemUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectItemUser",
                columns: table => new
                {
                    ProjectItemID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Write = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItemUser", x => new { x.ProjectItemID, x.UserID });
                    table.ForeignKey(
                        name: "FK_ProjectItemUser_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectItemUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItemUser_UserID",
                table: "ProjectItemUser",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectItemUser");
        }
    }
}
