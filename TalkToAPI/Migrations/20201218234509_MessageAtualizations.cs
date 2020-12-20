using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TalkToAPI.Migrations
{
    public partial class MessageAtualizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removed",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
