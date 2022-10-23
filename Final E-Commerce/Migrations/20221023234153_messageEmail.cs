using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class messageEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Messages");
        }
    }
}
