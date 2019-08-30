using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class LiveUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "CleaningPlan",
               columns: table => new
               {
                   ID = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   Description = table.Column<string>(nullable: true),
                   HasFloors = table.Column<bool>(nullable: false),
                   TranslationID = table.Column<int>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_CleaningPlan", x => x.ID);
               });

            migrationBuilder.Sql("INSERT INTO CleaningPlan (Description, HasFloors, TranslationID) VALUES ('Regular', 1, NULL)");
            migrationBuilder.Sql("INSERT INTO CleaningPlan (Description, HasFloors, TranslationID) VALUES ('Window', 0, NULL)");
            migrationBuilder.Sql("INSERT INTO CleaningPlan (Description, HasFloors, TranslationID) VALUES ('Fan coil', 0, NULL)");

            migrationBuilder.AddColumn<int>(
                name: "CleaningPlanID",
                table: "Area",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE a SET a.cleaningplanid = ct.PlanType FROM [Area] a INNER JOIN [CleaningTask] ct on a.id = ct.areaid");


            migrationBuilder.CreateIndex(
                name: "IX_Area_CleaningPlanID",
                table: "Area",
                column: "CleaningPlanID");

            /*migrationBuilder.AddForeignKey(
                name: "FK_Area_CleaningPlan_CleaningPlanID",
                table: "Area",
                column: "CleaningPlanID",
                principalTable: "CleaningPlan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);*/



            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Floor_FloorID",
                table: "CleaningTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_User_HRContactID",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CleaningDescription");

            migrationBuilder.DropColumn(
                name: "Decide",
                table: "UserPermissions");

            /*migrationBuilder.DropColumn(
                name: "Title",
                table: "User");*/

            migrationBuilder.DropColumn(
                name: "Decide",
                table: "PermissionsTemplate");

            /*migrationBuilder.DropColumn(
                name: "PlanType",
                table: "CleaningTask");*/

            migrationBuilder.RenameColumn(
                name: "HRContactID",
                table: "Customer",
                newName: "KeyAccountManagerID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_HRContactID",
                table: "Customer",
                newName: "IX_Customer_KeyAccountManagerID");

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageLocation",
                table: "QualityReportItem",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "QualityReport",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "RatingID",
                table: "QualityReport",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentFolderID",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentFolderID",
                table: "Customer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FloorID",
                table: "CleaningTask",
                nullable: true,
                oldClrType: typeof(int));

            

            migrationBuilder.CreateTable(
                name: "DocumentFolder",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentDocumentFolderID = table.Column<int>(nullable: true),
                    Standard = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VisibleToAllRoles = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFolder", x => x.ID);
                });

            migrationBuilder.Sql("INSERT INTO DocumentFolder (title, standard, VisibleToAllRoles) SELECT name, 0 as standard, 0 as VisibleToAllRoles FROM Customer");


            migrationBuilder.Sql("UPDATE c SET c.DocumentFolderID = df.id FROM DocumentFolder df INNER JOIN Customer c ON c.name = df.title");

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
                name: "Rating",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RatingItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    RatedByID = table.Column<int>(nullable: false),
                    RatingID = table.Column<int>(nullable: false),
                    TimeRated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RatingItem_User_RatedByID",
                        column: x => x.RatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingItem_Rating_RatingID",
                        column: x => x.RatingID,
                        principalTable: "Rating",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerID",
                table: "User",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReport_RatingID",
                table: "QualityReport",
                column: "RatingID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_DocumentFolderID",
                table: "Customer",
                column: "DocumentFolderID");

            

            migrationBuilder.CreateIndex(
                name: "IX_RatingItem_RatedByID",
                table: "RatingItem",
                column: "RatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_RatingItem_RatingID",
                table: "RatingItem",
                column: "RatingID");


            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Floor_FloorID",
                table: "CleaningTask",
                column: "FloorID",
                principalTable: "Floor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_DocumentFolder_DocumentFolderID",
                table: "Customer",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_User_KeyAccountManagerID",
                table: "Customer",
                column: "KeyAccountManagerID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            /*migrationBuilder.AddForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location",
                column: "DocumentFolderID",
                principalTable: "DocumentFolder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);*/

            migrationBuilder.AddForeignKey(
                name: "FK_QualityReport_Rating_RatingID",
                table: "QualityReport",
                column: "RatingID",
                principalTable: "Rating",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Customer_CustomerID",
                table: "User",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("UPDATE u SET u.RoleID = r.id FROM Role r INNER JOIN [User] u ON u.title = r.Name");


            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(@"UPDATE cleaningtask SET floorid = null WHERE plantype != 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_CleaningPlan_CleaningPlanID",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Floor_FloorID",
                table: "CleaningTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_DocumentFolder_DocumentFolderID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_User_KeyAccountManagerID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_DocumentFolder_DocumentFolderID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityReport_Rating_RatingID",
                table: "QualityReport");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Customer_CustomerID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User");


            migrationBuilder.DropTable(
                name: "CleaningPlan");

            migrationBuilder.DropTable(
                name: "DocumentFolder");

            migrationBuilder.DropTable(
                name: "DocumentItem");

            migrationBuilder.DropTable(
                name: "RatingItem");

            migrationBuilder.DropTable(
                name: "Rating");


            migrationBuilder.DropIndex(
                name: "IX_User_CustomerID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_QualityReport_RatingID",
                table: "QualityReport");

            migrationBuilder.DropIndex(
                name: "IX_Location_DocumentFolderID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Customer_DocumentFolderID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Area_CleaningPlanID",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageLocation",
                table: "QualityReportItem");

            migrationBuilder.DropColumn(
                name: "RatingID",
                table: "QualityReport");

            migrationBuilder.DropColumn(
                name: "DocumentFolderID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "DocumentFolderID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CleaningPlanID",
                table: "Area");

            migrationBuilder.RenameColumn(
                name: "KeyAccountManagerID",
                table: "Customer",
                newName: "HRContactID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_KeyAccountManagerID",
                table: "Customer",
                newName: "IX_Customer_HRContactID");

            migrationBuilder.AddColumn<bool>(
                name: "Decide",
                table: "UserPermissions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "QualityReport",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Decide",
                table: "PermissionsTemplate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "FloorID",
                table: "CleaningTask",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanType",
                table: "CleaningTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CleaningDescription",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    PricePerSquareMeter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningDescription", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Floor_FloorID",
                table: "CleaningTask",
                column: "FloorID",
                principalTable: "Floor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_User_HRContactID",
                table: "Customer",
                column: "HRContactID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
