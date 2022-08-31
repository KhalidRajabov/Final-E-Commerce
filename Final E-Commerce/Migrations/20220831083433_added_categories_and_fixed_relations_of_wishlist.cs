using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class added_categories_and_fixed_relations_of_wishlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CustomBrands_CustomBrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Products_ProductId",
                table: "Wishlists");

            migrationBuilder.DropTable(
                name: "CustomBrands");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Products_CustomBrandId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Popular",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedTime", "DeletedAt", "ImageUrl", "IsDeleted", "LastUpdatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, null, null, null, false, null, "Phones and Tablets", null },
                    { 2, null, null, null, false, null, "Watches", null },
                    { 3, null, null, null, false, null, "TV, Audio and Video", null },
                    { 4, null, null, null, false, null, "Computers and accessories", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedTime", "DeletedAt", "ImageUrl", "IsDeleted", "LastUpdatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { 5, null, null, null, false, null, "Smartphones", 1 },
                    { 6, null, null, null, false, null, "Tablets", 1 },
                    { 7, null, null, null, false, null, "Mobile Accessories", 1 },
                    { 8, null, null, null, false, null, "Smart watches", 2 },
                    { 9, null, null, null, false, null, "Analog watches", 2 },
                    { 10, null, null, null, false, null, "Kids watch", 2 },
                    { 11, null, null, null, false, null, "Watch Accessories", 2 },
                    { 12, null, null, null, false, null, "TVs", 3 },
                    { 13, null, null, null, false, null, "TV/Audio Video Accessories", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_WishlistId",
                table: "Products",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Wishlists_WishlistId",
                table: "Products",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Wishlists_WishlistId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WishlistId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Popular",
                table: "Brands");

            migrationBuilder.CreateTable(
                name: "CustomBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomBrands", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "d41fbc20-8be6-4cee-8272-59bea70a510d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "c17362b2-5344-44e5-be9d-9e08c3b455c2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "f197279b-c577-4781-8db7-ceb098d650b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "64415651-5959-4412-90b5-19a8e95999f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "4106b24b-2bc2-45e9-918c-2ecf86bf7a00");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CustomBrandId",
                table: "Products",
                column: "CustomBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CustomBrands_CustomBrandId",
                table: "Products",
                column: "CustomBrandId",
                principalTable: "CustomBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Products_ProductId",
                table: "Wishlists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
