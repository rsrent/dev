using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent.Migrations
{
    public partial class WorkAndFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_Contract_ContractAgreementID_ContractUserID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_Contract_ContractAgreementID_ContractUserID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_WorkContract_ContractAgreementID_ContractUserID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_Work_ContractAgreementID_ContractUserID",
                table: "Work");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractAgreementID",
                table: "WorkContract");

            migrationBuilder.DropColumn(
                name: "ContractUserID",
                table: "WorkContract");

            migrationBuilder.DropColumn(
                name: "ContractAgreementID",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "ContractUserID",
                table: "Work");

            migrationBuilder.AlterColumn<int>(
                name: "ContractID",
                table: "WorkContract",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "WorkContract",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Work",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Modifications",
                table: "Work",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Contract",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReplacement_ContractID",
                table: "WorkReplacement",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_ContractID",
                table: "WorkContract",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_ContractID",
                table: "Work",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_AgreementID",
                table: "Contract",
                column: "AgreementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Contract_ContractID",
                table: "Work",
                column: "ContractID",
                principalTable: "Contract",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_Contract_ContractID",
                table: "WorkContract",
                column: "ContractID",
                principalTable: "Contract",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkReplacement_Contract_ContractID",
                table: "WorkReplacement",
                column: "ContractID",
                principalTable: "Contract",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_Contract_ContractID",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkContract_Contract_ContractID",
                table: "WorkContract");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkReplacement_Contract_ContractID",
                table: "WorkReplacement");

            migrationBuilder.DropIndex(
                name: "IX_WorkReplacement_ContractID",
                table: "WorkReplacement");

            migrationBuilder.DropIndex(
                name: "IX_WorkContract_ContractID",
                table: "WorkContract");

            migrationBuilder.DropIndex(
                name: "IX_Work_ContractID",
                table: "Work");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_AgreementID",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "WorkContract");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "Modifications",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Contract");

            migrationBuilder.AlterColumn<int>(
                name: "ContractID",
                table: "WorkContract",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractAgreementID",
                table: "WorkContract",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractUserID",
                table: "WorkContract",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractAgreementID",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractUserID",
                table: "Work",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                columns: new[] { "AgreementID", "UserID" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkContract_ContractAgreementID_ContractUserID",
                table: "WorkContract",
                columns: new[] { "ContractAgreementID", "ContractUserID" });

            migrationBuilder.CreateIndex(
                name: "IX_Work_ContractAgreementID_ContractUserID",
                table: "Work",
                columns: new[] { "ContractAgreementID", "ContractUserID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Contract_ContractAgreementID_ContractUserID",
                table: "Work",
                columns: new[] { "ContractAgreementID", "ContractUserID" },
                principalTable: "Contract",
                principalColumns: new[] { "AgreementID", "UserID" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkContract_Contract_ContractAgreementID_ContractUserID",
                table: "WorkContract",
                columns: new[] { "ContractAgreementID", "ContractUserID" },
                principalTable: "Contract",
                principalColumns: new[] { "AgreementID", "UserID" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
