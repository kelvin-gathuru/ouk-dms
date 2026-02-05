using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Update_PageAction_Code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE `PageActions` pa
                INNER JOIN `ScreenOperations` so ON pa.Id = so.Id
                INNER JOIN `Operations` o ON so.OperationId = o.Id
                SET pa.Code = REPLACE(o.Name, ' ', '_');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
