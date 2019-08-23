using Microsoft.EntityFrameworkCore.Migrations;

namespace Kosystem.Web.Migrations
{
    public partial class RenameUserInQueueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserInQueueEntity",
                newName: "UsersInQueue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UsersInQueue",
                newName: "UserInQueueEntity");
        }
    }
}
