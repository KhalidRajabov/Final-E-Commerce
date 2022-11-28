using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class subjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 1, false, "News" });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 2, false, "Fashion" });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { 3, false, "Life Style" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
