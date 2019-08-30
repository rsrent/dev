using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class qrp_comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "QualityReport",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "QualityReport");
        }
    }
}
