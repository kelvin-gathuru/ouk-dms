using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Version_V5_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Users_CreatedBy",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "IsEnableSSL",
                table: "EmailSMTPSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "FileRequestDocumentId",
                table: "UserNotifications",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowInstanceId",
                table: "UserNotifications",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "EncryptionType",
                table: "EmailSMTPSettings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FromEmail",
                table: "EmailSMTPSettings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FromName",
                table: "EmailSMTPSettings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "DocumentVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "SignById",
                table: "DocumentVersions",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignDate",
                table: "DocumentVersions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Documents",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Documents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsAddedPageIndxing",
                table: "Documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchive",
                table: "Documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignatureExists",
                table: "Documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SignById",
                table: "Documents",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "SignDate",
                table: "Documents",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "DocumentAuditTrails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "AllowPdfSignature",
                table: "CompanyProfiles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AllowFileExtensions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    Extension = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowFileExtensions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompanyName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactPerson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
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
                    table.PrimaryKey("PK_Clients", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentIndexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentIndexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentIndexes_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentSignatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SignatureUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SignatureUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignatureDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSignatures_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSignatures_Users_SignatureUserId",
                        column: x => x.SignatureUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Subject = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxDocument = table.Column<int>(type: "int", nullable: true),
                    SizeInMb = table.Column<int>(type: "int", nullable: true),
                    AllowExtension = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileRequestStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LinkExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsLinkExpired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatTableSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ScreenName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SettingsJson = table.Column<string>(type: "longtext", nullable: true)
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
                    table.PrimaryKey("PK_MatTableSettings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PendingTransitions",
                columns: table => new
                {
                    TransitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FromStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ToStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TransitionName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingTransitions", x => x.TransitionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsWorkflowSetup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
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
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workflows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileRequestDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileRequestId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileRequestDocumentStatus = table.Column<int>(type: "int", nullable: false),
                    ApprovedRejectedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovalOrRjectedById = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRequestDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileRequestDocuments_FileRequests_FileRequestId",
                        column: x => x.FileRequestId,
                        principalTable: "FileRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileRequestDocuments_Users_ApprovalOrRjectedById",
                        column: x => x.ApprovalOrRjectedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InitiatedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowInstances_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowInstances_Users_InitiatedId",
                        column: x => x.InitiatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowInstances_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StepName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsSignatureRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFinal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkflowStepInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowInstanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_WorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowStepInstances_WorkflowSteps_StepId",
                        column: x => x.StepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "WorkflowTransitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FromStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ToStepId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Condition = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsFirstTransaction = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: true),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    Minutes = table.Column<int>(type: "int", nullable: true),
                    IsUploadDocumentVersion = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_WorkflowSteps_FromStepId",
                        column: x => x.FromStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_WorkflowSteps_ToStepId",
                        column: x => x.ToStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkflowInstanceEmailSenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowStepInstanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowTransitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstanceEmailSenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowInstanceEmailSenders_WorkflowStepInstances_WorkflowS~",
                        column: x => x.WorkflowStepInstanceId,
                        principalTable: "WorkflowStepInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowInstanceEmailSenders_WorkflowTransitions_WorkflowTra~",
                        column: x => x.WorkflowTransitionId,
                        principalTable: "WorkflowTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkflowTransitionInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkflowTransitionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkflowInstanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PerformById = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTransitionInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitionInstances_Users_PerformById",
                        column: x => x.PerformById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowTransitionInstances_WorkflowInstances_WorkflowInstan~",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitionInstances_WorkflowTransitions_WorkflowTran~",
                        column: x => x.WorkflowTransitionId,
                        principalTable: "WorkflowTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_FileRequestDocumentId",
                table: "UserNotifications",
                column: "FileRequestDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_WorkflowInstanceId",
                table: "UserNotifications",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_SignById",
                table: "DocumentVersions",
                column: "SignById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClientId",
                table: "Documents",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SignById",
                table: "Documents",
                column: "SignById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentIndexes_DocumentId",
                table: "DocumentIndexes",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatures_DocumentId",
                table: "DocumentSignatures",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatures_SignatureUserId",
                table: "DocumentSignatures",
                column: "SignatureUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRequestDocuments_ApprovalOrRjectedById",
                table: "FileRequestDocuments",
                column: "ApprovalOrRjectedById");

            migrationBuilder.CreateIndex(
                name: "IX_FileRequestDocuments_FileRequestId",
                table: "FileRequestDocuments",
                column: "FileRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRequests_CreatedById",
                table: "FileRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstanceEmailSenders_WorkflowStepInstanceId",
                table: "WorkflowInstanceEmailSenders",
                column: "WorkflowStepInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstanceEmailSenders_WorkflowTransitionId",
                table: "WorkflowInstanceEmailSenders",
                column: "WorkflowTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_DocumentId",
                table: "WorkflowInstances",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_InitiatedId",
                table: "WorkflowInstances",
                column: "InitiatedId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_WorkflowId",
                table: "WorkflowInstances",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_UserId",
                table: "Workflows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstances_StepId",
                table: "WorkflowStepInstances",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstances_UserId",
                table: "WorkflowStepInstances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepInstances_WorkflowInstanceId",
                table: "WorkflowStepInstances",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepRoles_RoleId",
                table: "WorkFlowStepRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowId",
                table: "WorkflowSteps",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepUsers_UserId",
                table: "WorkflowStepUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitionInstances_PerformById",
                table: "WorkflowTransitionInstances",
                column: "PerformById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitionInstances_WorkflowInstanceId",
                table: "WorkflowTransitionInstances",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitionInstances_WorkflowTransitionId",
                table: "WorkflowTransitionInstances",
                column: "WorkflowTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_FromStepId",
                table: "WorkflowTransitions",
                column: "FromStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_ToStepId",
                table: "WorkflowTransitions",
                column: "ToStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_WorkflowId",
                table: "WorkflowTransitions",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Clients_ClientId",
                table: "Documents",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_SignById",
                table: "Documents",
                column: "SignById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Users_CreatedBy",
                table: "DocumentVersions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Users_SignById",
                table: "DocumentVersions",
                column: "SignById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_FileRequestDocuments_FileRequestDocumentId",
                table: "UserNotifications",
                column: "FileRequestDocumentId",
                principalTable: "FileRequestDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_WorkflowInstances_WorkflowInstanceId",
                table: "UserNotifications",
                column: "WorkflowInstanceId",
                principalTable: "WorkflowInstances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Clients_ClientId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_SignById",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Users_CreatedBy",
                table: "DocumentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Users_SignById",
                table: "DocumentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_FileRequestDocuments_FileRequestDocumentId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_WorkflowInstances_WorkflowInstanceId",
                table: "UserNotifications");

            migrationBuilder.DropTable(
                name: "AllowFileExtensions");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "CustomCategories");

            migrationBuilder.DropTable(
                name: "DocumentIndexes");

            migrationBuilder.DropTable(
                name: "DocumentSignatures");

            migrationBuilder.DropTable(
                name: "FileRequestDocuments");

            migrationBuilder.DropTable(
                name: "MatTableSettings");

            migrationBuilder.DropTable(
                name: "PendingTransitions");

            migrationBuilder.DropTable(
                name: "WorkflowInstanceEmailSenders");

            migrationBuilder.DropTable(
                name: "WorkFlowStepRoles");

            migrationBuilder.DropTable(
                name: "WorkflowStepUsers");

            migrationBuilder.DropTable(
                name: "WorkflowTransitionInstances");

            migrationBuilder.DropTable(
                name: "FileRequests");

            migrationBuilder.DropTable(
                name: "WorkflowStepInstances");

            migrationBuilder.DropTable(
                name: "WorkflowTransitions");

            migrationBuilder.DropTable(
                name: "WorkflowInstances");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_FileRequestDocumentId",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_WorkflowInstanceId",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVersions_SignById",
                table: "DocumentVersions");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ClientId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_SignById",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileRequestDocumentId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "WorkflowInstanceId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "EncryptionType",
                table: "EmailSMTPSettings");

            migrationBuilder.DropColumn(
                name: "FromEmail",
                table: "EmailSMTPSettings");

            migrationBuilder.DropColumn(
                name: "FromName",
                table: "EmailSMTPSettings");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "SignById",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "SignDate",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsAddedPageIndxing",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsArchive",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsSignatureExists",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "SignById",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "SignDate",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "DocumentAuditTrails");

            migrationBuilder.DropColumn(
                name: "AllowPdfSignature",
                table: "CompanyProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableSSL",
                table: "EmailSMTPSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Users_CreatedBy",
                table: "DocumentVersions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
