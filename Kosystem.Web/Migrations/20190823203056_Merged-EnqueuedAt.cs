using Microsoft.EntityFrameworkCore.Migrations;

namespace Kosystem.Web.Migrations
{
    public partial class MergedEnqueuedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersInQueue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChangedAt",
                table: "Users",
                newName: "EnqueuedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                columns: new[] { "Id", "RoomId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "EnqueuedAt",
                table: "Users",
                newName: "ChangedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsersInQueue",
                columns: table => new
                {
                    RoomId = table.Column<string>(maxLength: 128, nullable: false),
                    UserId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInQueue", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersInQueue_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInQueue_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInQueue_UserId",
                table: "UsersInQueue",
                column: "UserId");
        }
    }
}
