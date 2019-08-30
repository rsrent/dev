using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class divisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DivisionID",
                table: "Units",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DivisionUser",
                columns: table => new
                {
                    DivisionID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionUser", x => new { x.DivisionID, x.UserID });
                    table.ForeignKey(
                        name: "FK_DivisionUser_Division_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Division",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DivisionUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_DivisionID",
                table: "Units",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionUser_UserID",
                table: "DivisionUser",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Division_DivisionID",
                table: "Units",
                column: "DivisionID",
                principalTable: "Division",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Division_DivisionID",
                table: "Units");

            migrationBuilder.DropTable(
                name: "DivisionUser");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropIndex(
                name: "IX_Units_DivisionID",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DivisionID",
                table: "Units");
        }
    }
}
