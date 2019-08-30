using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class WorkReplacementIDPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_WorkReplacement_ID",
                table: "WorkReplacement",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_WorkReplacement_ID",
                table: "WorkReplacement");
        }
    }
}
