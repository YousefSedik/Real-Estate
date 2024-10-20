using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStats.Migrations
{
    /// <inheritdoc />
    public partial class ContractFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.CreateTable(
                name: "ContractFiles",
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
                    table.PrimaryKey("PK_ContractFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractFiles_LeaseAgreement_LeaseAgreementId",
                        column: x => x.LeaseAgreementId,
                        principalTable: "LeaseAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractFiles_LeaseAgreementId",
                table: "ContractFiles",
                column: "LeaseAgreementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractFiles");

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseAgreementId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_File_LeaseAgreementId",
                table: "File",
                column: "LeaseAgreementId");
        }
    }
}
