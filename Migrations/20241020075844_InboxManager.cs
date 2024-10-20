using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class InboxManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxManager_LeaseAgreement_LeaseAgreementId",
                table: "InboxManager");

            migrationBuilder.DropForeignKey(
                name: "FK_InboxManager_Managers_ManagerId",
                table: "InboxManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxManager",
                table: "InboxManager");

            migrationBuilder.RenameTable(
                name: "InboxManager",
                newName: "InboxManagers");

            migrationBuilder.RenameIndex(
                name: "IX_InboxManager_ManagerId",
                table: "InboxManagers",
                newName: "IX_InboxManagers_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_InboxManager_LeaseAgreementId",
                table: "InboxManagers",
                newName: "IX_InboxManagers_LeaseAgreementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxManagers",
                table: "InboxManagers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxManagers_LeaseAgreement_LeaseAgreementId",
                table: "InboxManagers",
                column: "LeaseAgreementId",
                principalTable: "LeaseAgreement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboxManagers_Managers_ManagerId",
                table: "InboxManagers",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxManagers_LeaseAgreement_LeaseAgreementId",
                table: "InboxManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_InboxManagers_Managers_ManagerId",
                table: "InboxManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxManagers",
                table: "InboxManagers");

            migrationBuilder.RenameTable(
                name: "InboxManagers",
                newName: "InboxManager");

            migrationBuilder.RenameIndex(
                name: "IX_InboxManagers_ManagerId",
                table: "InboxManager",
                newName: "IX_InboxManager_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_InboxManagers_LeaseAgreementId",
                table: "InboxManager",
                newName: "IX_InboxManager_LeaseAgreementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxManager",
                table: "InboxManager",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxManager_LeaseAgreement_LeaseAgreementId",
                table: "InboxManager",
                column: "LeaseAgreementId",
                principalTable: "LeaseAgreement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboxManager_Managers_ManagerId",
                table: "InboxManager",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
