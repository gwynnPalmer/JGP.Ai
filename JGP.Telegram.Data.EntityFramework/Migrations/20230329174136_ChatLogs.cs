using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JGP.Telegram.Data.Migrations
{
    public partial class ChatLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ChatLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MessageDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatLogs_ChatId",
                schema: "dbo",
                table: "ChatLogs",
                column: "ChatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatLogs",
                schema: "dbo");
        }
    }
}
