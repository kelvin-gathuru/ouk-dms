using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Entry_Into_PageAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Insert PageAction
            migrationBuilder.Sql(@"
                INSERT INTO PageActions (Id, Name, `Order`, PageId, Code)
                SELECT 
                    rn.Id, 
                    CONCAT(rn.ScreenName, '_', rn.OperationName) AS Name,
                    rn.row_num AS `Order`,
                    rn.ScreenId AS PageId,
                    REPLACE(rn.OperationName, ' ', '_') AS Code
                FROM (
                    SELECT 
                        so.Id,
                        so.ScreenId,
                        s.Name AS ScreenName,
                        o.Name AS OperationName,
                        ROW_NUMBER() OVER (PARTITION BY so.ScreenId ORDER BY o.Name) AS row_num
                    FROM ScreenOperations so
                    INNER JOIN Screens s ON so.ScreenId = s.Id
                    INNER JOIN Operations o ON so.OperationId = o.Id
                ) rn;
            ");

            // Step 2: Update UserClaims to point to new PageAction.Id
            migrationBuilder.Sql(@"
                UPDATE UserClaims uc
                INNER JOIN ScreenOperations so
                    ON uc.OperationId = so.OperationId
                   AND uc.ScreenId = so.ScreenId
                SET uc.PageActionId = so.Id;
            ");

            // Step 3: Update RoleClaims to point to new PageAction.Id
            migrationBuilder.Sql(@"
                UPDATE RoleClaims rc
                INNER JOIN ScreenOperations so
                    ON rc.OperationId = so.OperationId
                   AND rc.ScreenId = so.ScreenId
                SET rc.PageActionId = so.Id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
