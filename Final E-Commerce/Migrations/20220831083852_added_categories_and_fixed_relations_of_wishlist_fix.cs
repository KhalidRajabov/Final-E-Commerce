using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class added_categories_and_fixed_relations_of_wishlist_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomBrandId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "dfca269f-2550-455b-945c-635635f4fce0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "eea51aee-e27e-4e3c-a1d9-caca52fb50d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "de0f9bbb-7730-4895-bc63-d576c066f7d6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "a8ba9212-2ae8-4fe1-aa51-c2b4e9a3ae66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "e75eb957-b667-4e45-b2c4-927e6a34504d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomBrandId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "64989ee1-a47c-415a-9511-b14d09754a6a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "5f76d238-deff-4c94-913a-4f6af3d05419");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "07bf9882-3303-4f7f-a87f-2b625bc27e60");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "f1646ce6-137e-482d-b10a-9466b59b1cd5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "569e78f3-189c-40c1-8cb1-fe6eca5693c4");
        }
    }
}
