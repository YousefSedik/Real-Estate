using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class AddNullToLeaseAgreementInTenantInBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxTenant_LeaseAgreement_LeaseAgreementId",
                table: "InboxTenant");

            migrationBuilder.AlterColumn<int>(
                name: "LeaseAgreementId",
                table: "InboxTenant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxTenant_LeaseAgreement_LeaseAgreementId",
                table: "InboxTenant",
                column: "LeaseAgreementId",
                principalTable: "LeaseAgreement",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxTenant_LeaseAgreement_LeaseAgreementId",
                table: "InboxTenant");

            migrationBuilder.AlterColumn<int>(
                name: "LeaseAgreementId",
                table: "InboxTenant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InboxTenant_LeaseAgreement_LeaseAgreementId",
                table: "InboxTenant",
                column: "LeaseAgreementId",
                principalTable: "LeaseAgreement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
