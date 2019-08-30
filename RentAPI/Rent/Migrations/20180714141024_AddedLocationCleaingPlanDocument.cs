using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class AddedLocationCleaingPlanDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationCleaningPlanDocument",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false),
                    CleaningPlanID = table.Column<int>(nullable: false),
                    DocumentFolderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCleaningPlanDocument", x => new { x.CleaningPlanID, x.DocumentFolderID, x.LocationID });
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_CleaningPlan_CleaningPlanID",
                        column: x => x.CleaningPlanID,
                        principalTable: "CleaningPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_DocumentFolder_DocumentFolderID",
                        column: x => x.DocumentFolderID,
                        principalTable: "DocumentFolder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LocationCleaningPlanDocument_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_LocationID",
                table: "CleaningTask",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCleaningPlanDocument_DocumentFolderID",
                table: "LocationCleaningPlanDocument",
                column: "DocumentFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCleaningPlanDocument_LocationID",
                table: "LocationCleaningPlanDocument",
                column: "LocationID");

            migrationBuilder.Sql("INSERT INTO LocationCleaningPlanDocument (LocationID, DocumentFolderID, CleaningPlanID)" +
                                 "SELECT Location.ID as LocationID, CleaningFolderID, 1 as CleaningPlanID FROM Location");
            migrationBuilder.Sql("INSERT INTO LocationCleaningPlanDocument (LocationID, DocumentFolderID, CleaningPlanID)" +
                                 "SELECT Location.ID as LocationID, WindowFolderID, 2 as CleaningPlanID FROM Location");
            migrationBuilder.Sql("INSERT INTO LocationCleaningPlanDocument (LocationID, DocumentFolderID, CleaningPlanID)" +
                                 "SELECT Location.ID as LocationID, FanCoilFolderID, 3 as CleaningPlanID FROM Location");
            migrationBuilder.Sql("INSERT INTO LocationCleaningPlanDocument (LocationID, DocumentFolderID, CleaningPlanID)" +
                                 "SELECT Location.ID as LocationID, PeriodicFolderID, 4 as CleaningPlanID FROM Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "LocationCleaningPlanDocument");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_LocationID",
                table: "CleaningTask");
        }
    }
}
