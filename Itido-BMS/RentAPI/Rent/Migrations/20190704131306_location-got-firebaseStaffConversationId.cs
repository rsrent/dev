using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class locationgotfirebaseStaffConversationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirestoreConversationStaff",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirestoreConversationStaff",
                table: "Location");
        }
    }
}
