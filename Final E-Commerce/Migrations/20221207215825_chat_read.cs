using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class chat_read : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {





            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadByReceiver = table.Column<bool>(type: "bit", nullable: false),
                    OtherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppuserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommunicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_AppuserId",
                        column: x => x.AppuserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatMessages_Communications_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_AppuserId",
                table: "ChatMessages",
                column: "AppuserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_CommunicationId",
                table: "ChatMessages",
                column: "CommunicationId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
