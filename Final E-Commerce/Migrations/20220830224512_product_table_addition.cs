using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class product_table_addition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Battery",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chipset",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Display",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontCamera",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Memory",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationSystem",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearCamera",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39792324-9bcf-41cc-aff7-9421ab090dbf",
                column: "ConcurrencyStamp",
                value: "1c65b73b-cb5f-413f-a5a0-aca8f33ac131");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64830485-9bcf-41fr-aff7-3333ab090dbf",
                column: "ConcurrencyStamp",
                value: "85fbecf3-5da3-4979-acc2-c324ab1a8347");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7985400a-d644-4954-a0c5-f579a46dd5c6",
                column: "ConcurrencyStamp",
                value: "08b8c541-f543-4f2d-90dd-ae89827b4313");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76fa29e-8b9b-431d-90e1-641c634654da",
                column: "ConcurrencyStamp",
                value: "97838a73-4257-4b45-bdfe-ceb70179fb25");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d78fa29e-8b9b-431d-90e1-312c634436da",
                column: "ConcurrencyStamp",
                value: "ec7994ec-31c0-489e-8607-492e4802b923");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Battery",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Chipset",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Display",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OperationSystem",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RearCamera",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");

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
        }
    }
}
