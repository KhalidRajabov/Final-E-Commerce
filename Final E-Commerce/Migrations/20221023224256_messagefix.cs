using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class messagefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Messages");
        }
    }
}
