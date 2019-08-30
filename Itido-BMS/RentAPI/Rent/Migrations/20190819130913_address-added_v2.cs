using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class addressadded_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Lon",
                table: "Address",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Lat",
                table: "Address",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Lon",
                table: "Address",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Lat",
                table: "Address",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
