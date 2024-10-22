using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyIdInInboxTenat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "InboxTenant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InboxTenant_PropertyId",
                table: "InboxTenant",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxTenant_Properities_PropertyId",
                table: "InboxTenant",
                column: "PropertyId",
                principalTable: "Properities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxTenant_Properities_PropertyId",
                table: "InboxTenant");

            migrationBuilder.DropIndex(
                name: "IX_InboxTenant_PropertyId",
                table: "InboxTenant");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "InboxTenant");
        }
    }
}
