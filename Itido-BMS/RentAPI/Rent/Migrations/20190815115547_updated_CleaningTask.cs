using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class updated_CleaningTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CleaningTask",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "CleaningTask",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "CleaningTask");
        }
    }
}
