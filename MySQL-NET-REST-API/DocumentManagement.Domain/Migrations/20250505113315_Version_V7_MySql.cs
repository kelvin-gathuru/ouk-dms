using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations;

/// <inheritdoc />
public partial class Version_V7_MySql : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "WorkFlowStepRoles");

        migrationBuilder.DropTable(
            name: "WorkflowStepUsers");

        migrationBuilder.DropForeignKey(
              name: "FK_DocumentVersions_Documents_DocumentId",
              table: "DocumentVersions");

        migrationBuilder.DropIndex(
            name: "IX_DocumentVersions_DocumentId",
            table: "DocumentVersions");

        migrationBuilder.DropColumn(
            name: "IsSignatureRequired",
            table: "WorkflowSteps");

        migrationBuilder.AddColumn<string>(
            name: "Color",
            table: "WorkflowTransitions",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<bool>(
            name: "IsSignatureRequired",
            table: "WorkflowTransitions",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "OrderNo",
            table: "WorkflowTransitions",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<bool>(
            name: "IsSuperAdmin",
            table: "Users",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<string>(
            name: "Url",
            table: "DocumentVersions",
            type: "varchar(255)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "longtext",
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<bool>(
            name: "IsShared",
            table: "Documents",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "LogoIconUrl",
            table: "CompanyProfiles",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<string>(
            name: "OpenAIAPIKey",
            table: "CompanyProfiles",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "AIPromptTemplates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                Name = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Description = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PromptInput = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AIPromptTemplates", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "UserOpenaiMsgs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                Title = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PromptInput = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Language = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                MaximumLength = table.Column<int>(type: "int", nullable: false),
                Creativity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                ToneOfVoice = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                SelectedModel = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                AiResponse = table.Column<string>(type: "longtext", nullable: true)
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
                table.PrimaryKey("PK_UserOpenaiMsgs", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserOpenaiMsgs_Users_CreatedBy",
                    column: x => x.CreatedBy,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "WorkflowTransitionRoles",
            columns: table => new
            {
                WorkflowTransitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkflowTransitionRoles", x => new { x.WorkflowTransitionId, x.RoleId });
                table.ForeignKey(
                    name: "FK_WorkflowTransitionRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkflowTransitionRoles_WorkflowTransitions_WorkflowTransiti~",
                    column: x => x.WorkflowTransitionId,
                    principalTable: "WorkflowTransitions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "WorkflowTransitionUsers",
            columns: table => new
            {
                WorkflowTransitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkflowTransitionUsers", x => new { x.WorkflowTransitionId, x.UserId });
                table.ForeignKey(
                    name: "FK_WorkflowTransitionUsers_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkflowTransitionUsers_WorkflowTransitions_WorkflowTransiti~",
                    column: x => x.WorkflowTransitionId,
                    principalTable: "WorkflowTransitions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_DocumentVersions_DocumentId_Url_CreatedBy",
            table: "DocumentVersions",
            columns: new[] { "DocumentId", "Url", "CreatedBy" });

        migrationBuilder.CreateIndex(
            name: "IX_UserOpenaiMsgs_CreatedBy",
            table: "UserOpenaiMsgs",
            column: "CreatedBy");

        migrationBuilder.CreateIndex(
            name: "IX_WorkflowTransitionRoles_RoleId",
            table: "WorkflowTransitionRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkflowTransitionUsers_UserId",
            table: "WorkflowTransitionUsers",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AIPromptTemplates");

        migrationBuilder.DropTable(
            name: "UserOpenaiMsgs");

        migrationBuilder.DropTable(
            name: "WorkflowTransitionRoles");

        migrationBuilder.DropTable(
            name: "WorkflowTransitionUsers");

        migrationBuilder.DropIndex(
            name: "IX_DocumentVersions_DocumentId_Url_CreatedBy",
            table: "DocumentVersions");

        migrationBuilder.DropColumn(
            name: "Color",
            table: "WorkflowTransitions");

        migrationBuilder.DropColumn(
            name: "IsSignatureRequired",
            table: "WorkflowTransitions");

        migrationBuilder.DropColumn(
            name: "OrderNo",
            table: "WorkflowTransitions");

        migrationBuilder.DropColumn(
            name: "IsSuperAdmin",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "IsShared",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "LogoIconUrl",
            table: "CompanyProfiles");

        migrationBuilder.DropColumn(
            name: "OpenAIAPIKey",
            table: "CompanyProfiles");

        migrationBuilder.AddColumn<bool>(
            name: "IsSignatureRequired",
            table: "WorkflowSteps",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<string>(
            name: "Url",
            table: "DocumentVersions",
            type: "longtext",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(255)",
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "WorkFlowStepRoles",
            columns: table => new
            {
                WorkflowStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkFlowStepRoles", x => new { x.WorkflowStepId, x.RoleId });
                table.ForeignKey(
                    name: "FK_WorkFlowStepRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkFlowStepRoles_WorkflowSteps_WorkflowStepId",
                    column: x => x.WorkflowStepId,
                    principalTable: "WorkflowSteps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "WorkflowStepUsers",
            columns: table => new
            {
                WorkflowStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkflowStepUsers", x => new { x.WorkflowStepId, x.UserId });
                table.ForeignKey(
                    name: "FK_WorkflowStepUsers_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkflowStepUsers_WorkflowSteps_WorkflowStepId",
                    column: x => x.WorkflowStepId,
                    principalTable: "WorkflowSteps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_DocumentVersions_DocumentId",
            table: "DocumentVersions",
            column: "DocumentId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkFlowStepRoles_RoleId",
            table: "WorkFlowStepRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkflowStepUsers_UserId",
            table: "WorkflowStepUsers",
            column: "UserId");
    }
}
