using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class SecurityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordsLastUpdated",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatePasswordsAtCronInterval",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlackListedToken",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TokenGuid = table.Column<string>(nullable: true),
                    BlackListTime = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    LoginID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListedToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BlackListedToken_Login_LoginID",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlackListedToken_LoginID",
                table: "BlackListedToken",
                column: "LoginID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackListedToken");

            migrationBuilder.DropColumn(
                name: "PasswordsLastUpdated",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UpdatePasswordsAtCronInterval",
                table: "Customer");
        }
    }
}
