using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class relationfixation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }
    }
}
