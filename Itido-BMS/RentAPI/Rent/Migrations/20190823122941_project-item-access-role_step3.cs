using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projectitemaccessrole_step3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "ProjectItem");

            migrationBuilder.CreateTable(
                name: "ProjectItemAccessTemplate",
                columns: table => new
                {
                    ProjectItemType = table.Column<int>(nullable: false),
                    ProjectRoleID = table.Column<int>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Write = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItemAccessTemplate", x => new { x.ProjectItemType, x.ProjectRoleID });
                    table.ForeignKey(
                        name: "FK_ProjectItemAccessTemplate_ProjectRole_ProjectRoleID",
                        column: x => x.ProjectRoleID,
                        principalTable: "ProjectRole",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItemAccessTemplate_ProjectRoleID",
                table: "ProjectItemAccessTemplate",
                column: "ProjectRoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectItemAccessTemplate");

            migrationBuilder.AddColumn<string>(
                name: "Access",
                table: "ProjectItem",
                maxLength: 20,
                nullable: true);
        }
    }
}
