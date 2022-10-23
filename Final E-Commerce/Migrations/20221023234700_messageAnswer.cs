using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class messageAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsweredDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "Messages");
        }
    }
}
