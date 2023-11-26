using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSms.Data.Migrations
{
    public partial class ChangeUpdatedAtPropNameInConversationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Conversations",
                newName: "UpdatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Conversations",
                newName: "ModifiedAt");
        }
    }
}
