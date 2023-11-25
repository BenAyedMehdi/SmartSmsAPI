using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSms.Data.Migrations
{
    public partial class IsReceivedInMessagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReceived",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceived",
                table: "Messages");
        }
    }
}
