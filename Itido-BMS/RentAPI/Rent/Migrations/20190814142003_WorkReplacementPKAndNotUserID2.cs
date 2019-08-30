using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class WorkReplacementPKAndNotUserID2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_WorkReplacement_ID",
                table: "WorkReplacement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_WorkReplacement_ID",
                table: "WorkReplacement",
                column: "ID");
        }
    }
}
