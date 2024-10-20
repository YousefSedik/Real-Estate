using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class AddModelFileAndInboxManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaseAgreementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_LeaseAgreement_LeaseAgreementId",
                        column: x => x.LeaseAgreementId,
                        principalTable: "LeaseAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InboxManager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    LeaseAgreementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxManager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboxManager_LeaseAgreement_LeaseAgreementId",
                        column: x => x.LeaseAgreementId,
                        principalTable: "LeaseAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InboxManager_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_LeaseAgreementId",
                table: "File",
                column: "LeaseAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxManager_LeaseAgreementId",
                table: "InboxManager",
                column: "LeaseAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxManager_ManagerId",
                table: "InboxManager",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "InboxManager");
        }
    }
}
