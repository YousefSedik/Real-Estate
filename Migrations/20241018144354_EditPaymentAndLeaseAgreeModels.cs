using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class EditPaymentAndLeaseAgreeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaseDuration",
                table: "TermsAndConditions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Payment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LeaseDuration",
                table: "LeaseAgreement",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "LeaseDuration",
                table: "LeaseAgreement");

            migrationBuilder.AddColumn<int>(
                name: "LeaseDuration",
                table: "TermsAndConditions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
