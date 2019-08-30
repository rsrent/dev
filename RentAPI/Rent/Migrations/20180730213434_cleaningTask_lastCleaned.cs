using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class cleaningTask_lastCleaned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerDocument");

            migrationBuilder.DropTable(
                name: "DocumentItem");

            migrationBuilder.DropTable(
                name: "LocationCleaningPlanDocument");

            migrationBuilder.AddColumn<int>(
                name: "LastTaskCompletedID",
                table: "CleaningTask",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE CleaningTask SET CleaningTask.LastTaskCompletedID = (SELECT C1.ID FROM CleaningTaskCompleted C1 WHERE C1.CompletedDate IN (SELECT MAX(C2.CompletedDate) FROM CleaningTaskCompleted C2 WHERE C2.CleaningTaskID = CleaningTask.ID))");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_LastTaskCompletedID",
                table: "CleaningTask",
                column: "LastTaskCompletedID");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_CleaningTaskCompleted_LastTaskCompletedID",
                table: "CleaningTask",
                column: "LastTaskCompletedID",
                principalTable: "CleaningTaskCompleted",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_CleaningTaskCompleted_LastTaskCompletedID",
                table: "CleaningTask");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_LastTaskCompletedID",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "LastTaskCompletedID",
                table: "CleaningTask");

            migrationBuilder.CreateTable(
                name: "CustomerDocument",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDocument", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentLocation = table.Column<string>(nullable: true),
                    RootDocumentFolderID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LocationCleaningPlanDocument",
                columns: table => new
                {
                    CleaningPlanID = table.Column<int>(nullable: false),
                    DocumentFolderID = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCleaningPlanDocument", x => new { x.CleaningPlanID, x.DocumentFolderID, x.LocationID });
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_CleaningPlan_CleaningPlanID",
                        column: x => x.CleaningPlanID,
                        principalTable: "CleaningPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_DocumentFolder_DocumentFolderID",
                        column: x => x.DocumentFolderID,
                        principalTable: "DocumentFolder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationCleaningPlanDocument_DocumentFolderID",
                table: "LocationCleaningPlanDocument",
                column: "DocumentFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCleaningPlanDocument_LocationID",
                table: "LocationCleaningPlanDocument",
                column: "LocationID");
        }
    }
}
