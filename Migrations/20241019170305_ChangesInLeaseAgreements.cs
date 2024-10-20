using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInLeaseAgreements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LeaseStatus",
                table: "LeaseAgreement",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "PersonalId",
                table: "LeaseAgreement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalId",
                table: "LeaseAgreement");

            migrationBuilder.AlterColumn<bool>(
                name: "LeaseStatus",
                table: "LeaseAgreement",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
