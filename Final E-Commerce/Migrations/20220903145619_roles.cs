using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39792324-9bcf-41cc-aff7-9421ab090dbf", "e911d4c6-c596-4e85-89ef-5b609f1d5cf8", "Member", null },
                    { "64830485-9bcf-41fr-aff7-3333ab090dbf", "9712d961-eeac-45db-a844-366fee1ccf69", "Moderator", null },
                    { "7985400a-d644-4954-a0c5-f579a46dd5c6", "809e5d07-ab90-48bc-87a3-2bc915fbb4df", "Admin", null },
                    { "d76fa29e-8b9b-431d-90e1-641c634654da", "6f6a6cc7-8aef-465b-9d4c-257b184f5d58", "SuperAdmin", null },
                    { "d78fa29e-8b9b-431d-90e1-312c634436da", "3322820b-563b-487a-9686-1b8b4aac00c6", "Editor", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
