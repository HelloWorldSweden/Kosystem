using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kosystem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.UniqueConstraint("AK_Rooms_DisplayId", x => x.DisplayId);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EnqueuedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RoomDisplayId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Rooms_RoomDisplayId",
                        column: x => x.RoomDisplayId,
                        principalTable: "Rooms",
                        principalColumn: "DisplayId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_RoomDisplayId",
                table: "People",
                column: "RoomDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DisplayId",
                table: "Rooms",
                column: "DisplayId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
