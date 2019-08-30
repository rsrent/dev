using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class CustomerCanReadLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanReadLogs",
                table: "Customer",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanReadLogs",
                table: "Customer");
        }
    }
}
