using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class wishlist_addition3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AppUserId",
                table: "Wishlists",
                column: "AppUserId",
                unique: true);
        }
    }
}
