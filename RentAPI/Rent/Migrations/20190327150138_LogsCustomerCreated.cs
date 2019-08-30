using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class LogsCustomerCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerCreated",
                table: "LocationLog",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCreated",
                table: "LocationLog");
        }
    }
}
