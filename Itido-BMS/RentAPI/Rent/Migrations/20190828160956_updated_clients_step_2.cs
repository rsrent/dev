using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updated_clients_step_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientID",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectItemID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ClientID",
                table: "User",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ProjectItemID",
                table: "Client",
                column: "ProjectItemID",
                unique: true,
                filter: "[ProjectItemID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Client_ClientID",
                table: "User",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Client_ClientID",
                table: "User");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropIndex(
                name: "IX_User_ClientID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "User");
        }
    }
}
