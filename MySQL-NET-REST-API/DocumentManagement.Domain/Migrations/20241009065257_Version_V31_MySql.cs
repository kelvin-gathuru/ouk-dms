using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Version_V31_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_CreatedBy",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_CreatedBy",
                table: "UserNotifications");

            migrationBuilder.AddColumn<int>(
                name: "NotificationsType",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "FromName",
                table: "SendEmails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ToName",
                table: "SendEmails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "Screens",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Logged",
                table: "NLog",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "IV",
                table: "DocumentVersions",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "DocumentVersions",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentStatusId",
                table: "Documents",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<byte[]>(
                name: "IV",
                table: "Documents",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "Documents",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StorageSettingId",
                table: "Documents",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "StorageType",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BannerUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentShareableLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LinkExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LinkCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLinkExpired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAllowDownload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentShareableLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentShareableLinks_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStatuses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PageHelpers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageHelpers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StorageSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JsonValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EnableEncryption = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StorageType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageSettings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentStatusId",
                table: "Documents",
                column: "DocumentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StorageSettingId",
                table: "Documents",
                column: "StorageSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentShareableLinks_DocumentId",
                table: "DocumentShareableLinks",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentStatuses_DocumentStatusId",
                table: "Documents",
                column: "DocumentStatusId",
                principalTable: "DocumentStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_StorageSettings_StorageSettingId",
                table: "Documents",
                column: "StorageSettingId",
                principalTable: "StorageSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentStatuses_DocumentStatusId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_StorageSettings_StorageSettingId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropTable(
                name: "DocumentShareableLinks");

            migrationBuilder.DropTable(
                name: "DocumentStatuses");

            migrationBuilder.DropTable(
                name: "PageHelpers");

            migrationBuilder.DropTable(
                name: "StorageSettings");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentStatusId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_StorageSettingId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NotificationsType",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "FromName",
                table: "SendEmails");

            migrationBuilder.DropColumn(
                name: "ToName",
                table: "SendEmails");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "IV",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "DocumentStatusId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IV",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "StorageSettingId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Logged",
                table: "NLog",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "CURRENT_TIMESTAMP")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_CreatedBy",
                table: "UserNotifications",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_CreatedBy",
                table: "UserNotifications",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
