using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class notis_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "User",
                maxLength: 2,
                nullable: true,
                defaultValue: "en");

            //migrationBuilder.AddColumn<string>(
                //name: "FirestoreConversationStaff",
                //table: "Location",
                //nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequest",
                table: "Absence",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Notis",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotiType = table.Column<string>(nullable: true),
                    SubjectID = table.Column<int>(nullable: false),
                    SendTime = table.Column<DateTime>(nullable: false),
                    SenderID = table.Column<int>(nullable: true),
                    ReceiverID = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notis", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notis_User_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notis_User_SenderID",
                        column: x => x.SenderID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notis_ReceiverID",
                table: "Notis",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Notis_SenderID",
                table: "Notis",
                column: "SenderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notis");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "User");

            //migrationBuilder.DropColumn(
                //name: "FirestoreConversationStaff",
                //table: "Location");

            migrationBuilder.DropColumn(
                name: "IsRequest",
                table: "Absence");
        }
    }
}
