using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Version_V6_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ClientSecretHash",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "UserNotifications",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentMetaTagId",
                table: "DocumentMetaDatas",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "MetaTagDate",
                table: "DocumentMetaDatas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "DocumentAuditTrails",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "DocumentAuditTrails",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchive",
                table: "CustomCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ArchiveParentId",
                table: "Categories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchive",
                table: "Categories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CategoryRolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsTimeBound = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAllowDownload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                    table.PrimaryKey("PK_CategoryRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryRolePermissions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryRolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryRolePermissions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryUserPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsTimeBound = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAllowDownload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                    table.PrimaryKey("PK_CategoryUserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryUserPermissions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryUserPermissions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryUserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentMetaTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEditable = table.Column<bool>(type: "tinyint(1)", nullable: false),
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
                    table.PrimaryKey("PK_DocumentMetaTags", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_CategoryId",
                table: "UserNotifications",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Name_CategoryId_IsArchive_IsDeleted",
                table: "Documents",
                columns: new[] { "Name", "CategoryId", "IsArchive", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentMetaDatas_DocumentMetaTagId",
                table: "DocumentMetaDatas",
                column: "DocumentMetaTagId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAuditTrails_CategoryId",
                table: "DocumentAuditTrails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_ParentId_IsArchive_IsDeleted",
                table: "Categories",
                columns: new[] { "Name", "ParentId", "IsArchive", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRolePermissions_CategoryId",
                table: "CategoryRolePermissions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRolePermissions_CreatedBy",
                table: "CategoryRolePermissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRolePermissions_RoleId",
                table: "CategoryRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUserPermissions_CategoryId",
                table: "CategoryUserPermissions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUserPermissions_CreatedBy",
                table: "CategoryUserPermissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUserPermissions_UserId",
                table: "CategoryUserPermissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_CreatedBy",
                table: "Categories",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAuditTrails_Categories_CategoryId",
                table: "DocumentAuditTrails",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentMetaDatas_DocumentMetaTags_DocumentMetaTagId",
                table: "DocumentMetaDatas",
                column: "DocumentMetaTagId",
                principalTable: "DocumentMetaTags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Categories_CategoryId",
                table: "UserNotifications",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_CreatedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAuditTrails_Categories_CategoryId",
                table: "DocumentAuditTrails");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentMetaDatas_DocumentMetaTags_DocumentMetaTagId",
                table: "DocumentMetaDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Categories_CategoryId",
                table: "UserNotifications");

            migrationBuilder.DropTable(
                name: "CategoryRolePermissions");

            migrationBuilder.DropTable(
                name: "CategoryUserPermissions");

            migrationBuilder.DropTable(
                name: "DocumentMetaTags");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_CategoryId",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Name_CategoryId_IsArchive_IsDeleted",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_DocumentMetaDatas_DocumentMetaTagId",
                table: "DocumentMetaDatas");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAuditTrails_CategoryId",
                table: "DocumentAuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_ParentId_IsArchive_IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClientSecretHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "DocumentMetaTagId",
                table: "DocumentMetaDatas");

            migrationBuilder.DropColumn(
                name: "MetaTagDate",
                table: "DocumentMetaDatas");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DocumentAuditTrails");

            migrationBuilder.DropColumn(
                name: "IsArchive",
                table: "CustomCategories");

            migrationBuilder.DropColumn(
                name: "ArchiveParentId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsArchive",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "DocumentAuditTrails",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
