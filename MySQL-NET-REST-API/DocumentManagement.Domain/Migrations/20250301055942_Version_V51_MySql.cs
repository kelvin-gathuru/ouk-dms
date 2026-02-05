using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Version_V51_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentIndexes_Documents_DocumentId",
                table: "DocumentIndexes");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Url",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_DocumentIndexes_DocumentId",
                table: "DocumentIndexes");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "DocumentVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllChunkUploaded",
                table: "DocumentVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChunk",
                table: "DocumentVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentVersion",
                table: "DocumentVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                table: "DocumentVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Documents",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "Documents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Documents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllChunkUploaded",
                table: "Documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChunk",
                table: "Documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentVersionId",
                table: "DocumentIndexes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "DocumentChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChunkIndex = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Extension = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentVersionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentChunks_DocumentVersions_DocumentVersionId",
                        column: x => x.DocumentVersionId,
                        principalTable: "DocumentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentIndexes_DocumentVersionId",
                table: "DocumentIndexes",
                column: "DocumentVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentChunks_DocumentVersionId",
                table: "DocumentChunks",
                column: "DocumentVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentIndexes_DocumentVersions_DocumentVersionId",
                table: "DocumentIndexes",
                column: "DocumentVersionId",
                principalTable: "DocumentVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentIndexes_DocumentVersions_DocumentVersionId",
                table: "DocumentIndexes");

            migrationBuilder.DropTable(
                name: "DocumentChunks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentIndexes_DocumentVersionId",
                table: "DocumentIndexes");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "IsAllChunkUploaded",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "IsChunk",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "IsCurrentVersion",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsAllChunkUploaded",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsChunk",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentVersionId",
                table: "DocumentIndexes");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Documents",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Url",
                table: "Documents",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentIndexes_DocumentId",
                table: "DocumentIndexes",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentIndexes_Documents_DocumentId",
                table: "DocumentIndexes",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
