using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class newname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_UserBlogs_UserBlogsId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogSubjects_UserBlogs_UserBlogId",
                table: "BlogSubjects");

            migrationBuilder.DropTable(
                name: "UserBlogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogSubjects_UserBlogId",
                table: "BlogSubjects");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_UserBlogsId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "UserBlogId",
                table: "BlogSubjects");

            migrationBuilder.DropColumn(
                name: "UserBlogsId",
                table: "BlogComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserBlogId",
                table: "BlogSubjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserBlogsId",
                table: "BlogComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserBlogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentCount = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBlogs_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogSubjects_UserBlogId",
                table: "BlogSubjects",
                column: "UserBlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_UserBlogsId",
                table: "BlogComments",
                column: "UserBlogsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogs_AppUserId",
                table: "UserBlogs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_UserBlogs_UserBlogsId",
                table: "BlogComments",
                column: "UserBlogsId",
                principalTable: "UserBlogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSubjects_UserBlogs_UserBlogId",
                table: "BlogSubjects",
                column: "UserBlogId",
                principalTable: "UserBlogs",
                principalColumn: "Id");
        }
    }
}
