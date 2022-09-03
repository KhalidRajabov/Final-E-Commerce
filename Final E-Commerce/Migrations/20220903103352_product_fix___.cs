using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class product_fix___ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "20 MP, f/2.0, (wide), 1/3, 0.9µm, 1080p@30fps", "12 MP, f/1.9, 1/2.55, 1.4µm, dual pixel PDAF, 4K@30/60fps, 1080p@30fps (gyro-EIS), 1080p@240fps, 720p@960fps" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "40 MP, f/2.2, 26mm (wide), 1/2.82, 0.7µm, PDAF", "108 MP, f/1.8, 23mm (wide), 1/1.33, 0.8µm, PDAF, Laser AF, OIS 10 MP, f/4.9, 230mm (periscope telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 10x optical zoom\r\n10 MP, f/2.4, 70mm (telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 3x optical zoom 12 MP, f/2.2, 13mm, 120˚ (ultrawide), 1/2.55\", 1.4µm, dual pixel PDAF, Super Steady video" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "12 MP, f/2.2, 23mm (wide), 1/3.6, 4K@24/25/30/60fps, 1080p@30/60/120fps, gyro-EIS", "48 MP, (wide), dual pixel PDAF, sensor-shift OIS 12 MP, f/2.8, 77mm (telephoto), PDAF, OIS, 3x optical zoom 12 MP, f/1.8, 13mm, 120˚ (ultrawide), 1.4µm, PDAF TOF 3D LiDAR scanner (depth)" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "20 MP, f/2.0, (wide), 1/3\", 0.9µm, 1080p@30fps", "12 MP, f/1.9, 1/2.55\", 1.4µm, dual pixel PDAF, 4K@30/60fps, 1080p@30fps (gyro-EIS), 1080p@240fps, 720p@960fps" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "40 MP, f/2.2, 26mm (wide), 1/2.82\", 0.7µm, PDAF", "108 MP, f/1.8, 23mm (wide), 1/1.33\", 0.8µm, PDAF, Laser AF, OIS\r\n10 MP, f/4.9, 230mm (periscope telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 10x optical zoom\r\n10 MP, f/2.4, 70mm (telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 3x optical zoom\r\n12 MP, f/2.2, 13mm, 120˚ (ultrawide), 1/2.55\", 1.4µm, dual pixel PDAF, Super Steady video" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FrontCamera", "RearCamera" },
                values: new object[] { "12 MP, f/2.2, 23mm (wide), 1/3.6\", 4K@24/25/30/60fps, 1080p@30/60/120fps, gyro-EIS\r\n", "48 MP, (wide), dual pixel PDAF, sensor-shift OIS\r\n12 MP, f/2.8, 77mm (telephoto), PDAF, OIS, 3x optical zoom\r\n12 MP, f/1.8, 13mm, 120˚ (ultrawide), 1.4µm, PDAF\r\nTOF 3D LiDAR scanner (depth)" });
        }
    }
}
