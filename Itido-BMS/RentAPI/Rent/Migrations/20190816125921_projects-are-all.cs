using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class projectsareall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_Project_ProjectID",
                table: "CleaningTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Location_LocationID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_LocationID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_ProjectID",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "WorkContract",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "QualityReport",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "LocationLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "DocumentFolder",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectItemID",
                table: "CleaningTask",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<int>(nullable: false),
                    ProjectItemType = table.Column<int>(nullable: false),
                    Access = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProjectItem_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectItemID = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comment_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirestoreConversation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectItemID = table.Column<int>(nullable: false),
                    ConversationID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirestoreConversation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FirestoreConversation_ProjectItem_ProjectItemID",
                        column: x => x.ProjectItemID,
                        principalTable: "ProjectItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_ProjectItemID",
                table: "WorkContract",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_ProjectItemID",
                table: "Work",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityReport_ProjectItemID",
                table: "QualityReport",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ParentID",
                table: "Project",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationLog_ProjectItemID",
                table: "LocationLog",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ProjectItemID",
                table: "Location",
                column: "ProjectItemID",
                unique: true,
                filter: "[ProjectItemID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProjectItemID",
                table: "Customer",
                column: "ProjectItemID",
                unique: true,
                filter: "[ProjectItemID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_ProjectItemID",
                table: "CleaningTask",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProjectItemID",
                table: "Comment",
                column: "ProjectItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FirestoreConversation_ProjectItemID",
                table: "FirestoreConversation",
                column: "ProjectItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItem_ProjectID",
                table: "ProjectItem",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_ProjectItem_ProjectItemID",
                table: "CleaningTask",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ProjectItem_ProjectItemID",
                table: "Customer",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFolder_ProjectItem_ProjectItemID",
                table: "DocumentFolder",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_ProjectItem_ProjectItemID",
                table: "Location",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLog_ProjectItem_ProjectItemID",
                table: "LocationLog",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Project_ParentID",
                table: "Project",
                column: "ParentID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityReport_ProjectItem_ProjectItemID",
                table: "QualityReport",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_ProjectItem_ProjectItemID",
                table: "Work",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_ProjectItem_ProjectItemID",
                table: "WorkContract",
                column: "ProjectItemID",
                principalTable: "ProjectItem",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTask_ProjectItem_ProjectItemID",
                table: "CleaningTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ProjectItem_ProjectItemID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFolder_ProjectItem_ProjectItemID",
                table: "DocumentFolder");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_ProjectItem_ProjectItemID",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationLog_ProjectItem_ProjectItemID",
                table: "LocationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Project_ParentID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityReport_ProjectItem_ProjectItemID",
                table: "QualityReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_ProjectItem_ProjectItemID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_ProjectItem_ProjectItemID",
                table: "WorkContract");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "FirestoreConversation");

            migrationBuilder.DropTable(
                name: "ProjectItem");

            migrationBuilder.DropIndex(
                name: "IX_WorkContract_ProjectItemID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_Work_ProjectItemID",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_QualityReport_ProjectItemID",
                table: "QualityReport");

            migrationBuilder.DropIndex(
                name: "IX_Project_ParentID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_LocationLog_ProjectItemID",
                table: "LocationLog");

            migrationBuilder.DropIndex(
                name: "IX_Location_ProjectItemID",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFolder_ProjectItemID",
                table: "DocumentFolder");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ProjectItemID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTask_ProjectItemID",
                table: "CleaningTask");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "WorkContract");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "QualityReport");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "LocationLog");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "DocumentFolder");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ProjectItemID",
                table: "CleaningTask");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_LocationID",
                table: "Project",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTask_ProjectID",
                table: "CleaningTask",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTask_Project_ProjectID",
                table: "CleaningTask",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Location_LocationID",
                table: "Project",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
