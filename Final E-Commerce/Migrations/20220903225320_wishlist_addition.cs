using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class wishlist_addition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wishlists_WishlistId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WishlistId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_AspNetUsers_AppUserId",
                table: "Wishlists",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_AspNetUsers_AppUserId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WishlistId",
                table: "AspNetUsers",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wishlists_WishlistId",
                table: "AspNetUsers",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }
    }
}
