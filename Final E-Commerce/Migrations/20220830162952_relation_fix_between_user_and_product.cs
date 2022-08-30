using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class relation_fix_between_user_and_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProduct");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "f9a408bb-5705-4a66-b38c-d5f24abdd1aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "158df72d-08ab-4230-8ccf-866ff6f93def");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "06f3ef09-af9c-475b-bf11-1f181f7ded95");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "7608222f-4d01-4d84-8d5e-bc14df32378c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "9467528e-aa3a-4fe6-9368-ecf21804f03c");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AppUserId",
                table: "Products",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AppUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "UserProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProduct_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "4b7c7ccb-c4fb-4c42-acde-a7f3f3cabbd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "dd68c955-d8c8-448c-8f97-5293810fbf0f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "e7530a27-e4fe-47c1-b12c-be4309d9fd63");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "4ce98f02-ab5e-4fe1-ac0b-c2cf82a8b24f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "312dbd17-68b7-463c-ae81-6797a4dc0364");

            migrationBuilder.CreateIndex(
                name: "IX_UserProduct_ProductId",
                table: "UserProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProduct_UserId",
                table: "UserProduct",
                column: "UserId");
        }
    }
}
