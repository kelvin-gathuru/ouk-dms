using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_CLAIM_VALUE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Start_Workflow' WHERE `Name`='Assigned Documents_Start Workflow';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Start_Workflow' WHERE `Name`='All Documents_Start Workflow';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Add_Signature' WHERE `Name`='All Documents_Add Signature';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Add_Signature' WHERE `Name`='Assigned Documents_Add Signature';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Archive_Download_Document' WHERE `Name`='Archive Documents_Download Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Download_Document' WHERE `Name`='All Documents_Download Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Download_Document' WHERE `Name`='Assigned Documents_Download Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Upload_New_version' WHERE `Name`='Assigned Documents_Upload New version';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Upload_New_version' WHERE `Name`='All Documents_Upload New version';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Manage_Indexing' WHERE `Name`='All Documents_Manage Indexing';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Manage_Indexing' WHERE `Name`='Assigned Documents_Manage Indexing';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Category_Share_Folder' WHERE `Name`='Document Category_Share Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Share_Folder' WHERE `Name`='Assigned Folders_Share Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Share_Folder' WHERE `Name`='All Folders_Share Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Remove_Share_Folder' WHERE `Name`='Assigned Documents_Remove Share Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Remove_Share_Folder' WHERE `Name`='All Documents_Remove Share Folder';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
