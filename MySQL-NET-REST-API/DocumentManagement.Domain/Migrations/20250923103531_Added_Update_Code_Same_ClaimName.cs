using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Added_Update_Code_Same_ClaimName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Add_Comment' WHERE `Name`='All Documents_Add Comment';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Add_Comment' WHERE `Name`='Assigned Documents_Add Comment';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Delete_Comment' WHERE `Name`='All Documents_Delete Comment';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Delete_Comment' WHERE `Name`='Assigned Documents_Delete Comment';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Remove_Share_Document' WHERE `Name`='All Documents_Remove Share Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Share_Document' WHERE `Name`='All Documents_Share Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Remove_Share_Document' WHERE `Name`='Assigned Documents_Remove Share Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Share_Document' WHERE `Name`='Assigned Documents_Share Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Create_Shareable_Link' WHERE `Name`='Assigned Documents_Create Shareable Link';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Archive_Create_Shareable_Link' WHERE `Name`='Archive Documents_Create Shareable Link';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Create_Shareable_Link' WHERE `Name`='All Documents_Create Shareable Link';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='all_view_version_history' WHERE `Name`='All Documents_View version history';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Archive_View_version_history' WHERE `Name`='Archive Documents_View version history';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_View_version_history' WHERE `Name`='Assigned Documents_View version history';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Restore_version' WHERE `Name`='All Documents_Restore version';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Restore_version' WHERE `Name`='Assigned Documents_Restore version';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Archive_Send_Email' WHERE `Name`='Archive Documents_Send Email';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Send_Email' WHERE `Name`='All Documents_Send Email';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Send_Email' WHERE `Name`='Assigned Documents_Send Email';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Add_Reminder' WHERE `Name`='All Documents_Add Reminder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Add_Reminder' WHERE `Name`='Assigned Documents_Add Reminder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Archive_Folder' WHERE `Name`='All Folders_Archive Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Category_Archive_Folder' WHERE `Name`='Document Category_Archive Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Archive_Folder' WHERE `Name`='Assigned Folders_Archive Folder';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Get_Document_Summary' WHERE `Name`='Assigned Documents_Get Document Summary';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='ALL_Get_Document_Summary' WHERE `Name`='All Documents_Get Document Summary';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_View_Documents' WHERE `Name`='Assigned Documents_View Documents';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_View_Documents' WHERE `Name`='All Documents_View Documents';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Create_Document' WHERE `Name`='All Documents_Create Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Create_Document' WHERE `Name`='Assigned Documents_Create Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Edit_Document' WHERE `Name`='Assigned Documents_Edit Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Edit_Document' WHERE `Name`='All Documents_Edit Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='Assigned_Archive_Document' WHERE `Name`='Assigned Documents_Archive Document';
            ");
            migrationBuilder.Sql(@"
                UPDATE `PageActions` SET `code`='All_Archive_Document' WHERE `Name`='All Documents_Archive Document';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
