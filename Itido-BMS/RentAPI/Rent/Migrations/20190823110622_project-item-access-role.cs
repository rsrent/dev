using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projectitemaccessrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectRoleID",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectRole",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProjectItemAccess",
                columns: table => new
                {
                    ProjectItemID = table.Column<int>(nullable: false),
                    ProjectRoleID = table.Column<int>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Write = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItemAccess", x => new { x.ProjectItemID, x.ProjectRoleID });
                    table.ForeignKey(
                        name: "FK_ProjectItemAccess_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectItemAccess_ProjectRole_ProjectRoleID",
                        column: x => x.ProjectRoleID,
                        principalTable: "ProjectRole",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ProjectRoleID",
                table: "User",
                column: "ProjectRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItemAccess_ProjectRoleID",
                table: "ProjectItemAccess",
                column: "ProjectRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ProjectRole_ProjectRoleID",
                table: "User",
                column: "ProjectRoleID",
                principalTable: "ProjectRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ProjectRole_ProjectRoleID",
                table: "User");

            migrationBuilder.DropTable(
                name: "ProjectItemAccess");

            migrationBuilder.DropTable(
                name: "ProjectRole");

            migrationBuilder.DropIndex(
                name: "IX_User_ProjectRoleID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProjectRoleID",
                table: "User");
        }
    }
}
