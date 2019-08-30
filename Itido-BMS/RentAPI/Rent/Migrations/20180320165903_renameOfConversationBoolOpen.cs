using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rent.Migrations
{
    public partial class renameOfConversationBoolOpen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Closed",
                table: "Conversation",
                newName: "Open");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Open",
                table: "Conversation",
                newName: "Closed");
        }
    }
}
