using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Added_Role_And_User_Claim_Remove_Screen_Operation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Operations_OperationId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Screens_ScreenId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Operations_OperationId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Screens_ScreenId",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_OperationId",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_ScreenId",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_OperationId",
                table: "RoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_ScreenId",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ScreenId",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ScreenId",
                table: "RoleClaims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OperationId",
                table: "UserClaims",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ScreenId",
                table: "UserClaims",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "OperationId",
                table: "RoleClaims",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ScreenId",
                table: "RoleClaims",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_OperationId",
                table: "UserClaims",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_ScreenId",
                table: "UserClaims",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_OperationId",
                table: "RoleClaims",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_ScreenId",
                table: "RoleClaims",
                column: "ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Operations_OperationId",
                table: "RoleClaims",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Screens_ScreenId",
                table: "RoleClaims",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Operations_OperationId",
                table: "UserClaims",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Screens_ScreenId",
                table: "UserClaims",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id");
        }
    }
}
