using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Version_V8_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "WorkflowSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemUser",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ArchiveById",
                table: "Documents",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "OnExpiryAction",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "RetentionDate",
                table: "Documents",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetentionPeriodInDays",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseKey",
                table: "CompanyProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseCode",
                table: "CompanyProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ArchiveById",
                table: "Categories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ArchiveRetentions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RetentionPeriodInDays = table.Column<int>(type: "int", nullable: true),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveRetentions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ArchiveById",
                table: "Documents",
                column: "ArchiveById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ArchiveById",
                table: "Categories",
                column: "ArchiveById");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_ArchiveById",
                table: "Categories",
                column: "ArchiveById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_ArchiveById",
                table: "Documents",
                column: "ArchiveById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_ArchiveById",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_ArchiveById",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "ArchiveRetentions");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ArchiveById",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ArchiveById",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "IsSystemUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ArchiveById",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "OnExpiryAction",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RetentionDate",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RetentionPeriodInDays",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LicenseKey",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "PurchaseCode",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "ArchiveById",
                table: "Categories");
        }
    }
}
