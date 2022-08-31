using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class bio_table_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Bios",
                columns: new[] { "Id", "Address", "Author", "Email", "Facebook", "Headertext", "Instagram", "Linkedin", "Logo", "NewsLetterHeader", "NewsLetterText", "Phone", "Twitter" },
                values: new object[] { 1, "Bakı şəhəri, Səbayıl rayonu, G.Əliyev küç.54D", "Khalid Rajabov", "khalidsr@code.edu.az", "https://www.facebook.com/khalid.radjabov.5", "Free shipping • Free 30 days return • Express delivery", "https://www.instagram.com/mr.felix_666/", "https://www.linkedin.com/mwlite/in/khalid-racabov-867281243", "logo.png", "Our Newsletter", "Subscribe to our Newsletter to receive early discount offers", "+994515186423", "https://twitter.com/slayer_dante_?t=G-VmlfAok6IC_vxmQnYfLQ&s=08" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Bios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39792324-9bcf-41cc-aff7-9421ab090dbf", "4f0dc56b-1e2f-45b2-81df-1fd9f23af516", "Member", null },
                    { "64830485-9bcf-41fr-aff7-3333ab090dbf", "34604860-6438-48e5-a00e-543fb4138d3e", "Moderator", null },
                    { "7985400a-d644-4954-a0c5-f579a46dd5c6", "db2e6887-a601-4369-a3e0-7441ac6fef53", "Admin", null },
                    { "d76fa29e-8b9b-431d-90e1-641c634654da", "43434b0f-ed3d-487f-8bd1-b240639556b6", "SuperAdmin", null },
                    { "d78fa29e-8b9b-431d-90e1-312c634436da", "0de114a9-4c83-411c-9d75-0fc2506dbcbb", "Editor", null }
                });
        }
    }
}
