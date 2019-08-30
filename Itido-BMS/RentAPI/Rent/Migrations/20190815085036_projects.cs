using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Area_AreaID",
                table: "CleaningTask");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "CleaningTask",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "CleaningTask",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Project_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectID, x.UserID });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_ProjectID",
                table: "CleaningTask",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_LocationID",
                table: "Project",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UserID",
                table: "ProjectUser",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Area_AreaID",
                table: "CleaningTask",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Project_ProjectID",
                table: "CleaningTask",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Area_AreaID",
                table: "CleaningTask");

            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Project_ProjectID",
                table: "CleaningTask");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_ProjectID",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "CleaningTask");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "CleaningTask",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Area_AreaID",
                table: "CleaningTask",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
