using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class addressadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectItemID = table.Column<int>(nullable: false),
                    AddressName = table.Column<string>(nullable: true),
                    Lat = table.Column<double>(nullable: false),
                    Lon = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ProjectItemID",
                table: "Address",
                column: "ProjectItemID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
