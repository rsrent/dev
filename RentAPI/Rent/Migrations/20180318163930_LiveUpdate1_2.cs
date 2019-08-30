using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class LiveUpdate1_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE Area SET CleaningPlanID = CleaningPlanID + 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_CleaningPlan_CleaningPlanID",
                table: "Area",
                column: "CleaningPlanID",
                principalTable: "CleaningPlan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "Title",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PlanType",
                table: "CleaningTask");

            migrationBuilder.Sql("INSERT INTO DocumentFolder (title, standard, VisibleToAllRoles) SELECT CONCAT('location-', Location.ID) as title, 0 as standard, 0 as VisibleToAllRoles FROM Location");

            migrationBuilder.Sql("UPDATE l SET l.DocumentFolderID = df.id FROM DocumentFolder df INNER JOIN Location l ON CONCAT('location-', l.ID) = df.title");


            migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
