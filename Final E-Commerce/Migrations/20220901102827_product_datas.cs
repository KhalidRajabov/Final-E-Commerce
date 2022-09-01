using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_E_Commerce.Migrations
{
    public partial class product_datas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GPU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "CreatedTime", "DeletedAt", "ImageUrl", "IsDeleted", "LastUpdatedAt", "Name", "Popular" },
                values: new object[,]
                {
                    { 1, null, null, "android.jpg", false, null, "Android", false },
                    { 2, null, null, "apple.jpg", false, null, "Apple", false }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "tabletsandphones.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "tv.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "computers.jpg");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AppUserId", "Battery", "Bestseller", "Body", "BrandId", "CategoryId", "Chipset", "Count", "CreatedTime", "DeletedAt", "Description", "DiscountPercent", "DiscountPrice", "Display", "FrontCamera", "GPU", "InStock", "IsDeleted", "IsFeatured", "LastUpdatedAt", "Memory", "Name", "NewArrival", "OperationSystem", "Price", "RearCamera", "ReleaseDate", "Views", "Weight" },
                values: new object[] { 1, null, "Li-Po 4000 mAh, non-removable, quick charge 3.0", false, "155.5 x 75.3 x 8.8 mm (6.12 x 2.96 x 0.35 in)", 1, 5, "Qualcomm SDM845 Snapdragon 845 (10 nm)", 0, null, null, "POCO F1 (Rosso Red, 128 GB) (6 GB RAM) Meet the POCO F1 - the first flagship smartphone from POCO by Xiaomi. The POCO F1 sports Qualcomm flagship Snapdragon 845 processor, an octa-core CPU with a maximum clock speed of 2.8 GHz which is supported by 6 GB of LPDDR4X RAM.", null, null, "6.18 inches, 96.2 cm2 (~82.2% screen-to-body ratio),1080 x 2246 pixels, 18.7:9 ratio (~403 ppi density)", "20 MP, f/2.0, (wide), 1/3\", 0.9µm, 1080p@30fps", "Adreno 630", false, false, false, null, "64GB 6GB RAM, 128GB 6GB RAM, 256GB 8GB RAM", "Xiaomi Pocophone F1", false, "Android 8.1 (Oreo), upgradable to Android 10, MIUI 12", 750.0, "12 MP, f/1.9, 1/2.55\", 1.4µm, dual pixel PDAF, 4K@30/60fps, 1080p@30fps (gyro-EIS), 1080p@240fps, 720p@960fps", new DateTime(2018, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "182 g (6.42 oz)" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AppUserId", "Battery", "Bestseller", "Body", "BrandId", "CategoryId", "Chipset", "Count", "CreatedTime", "DeletedAt", "Description", "DiscountPercent", "DiscountPrice", "Display", "FrontCamera", "GPU", "InStock", "IsDeleted", "IsFeatured", "LastUpdatedAt", "Memory", "Name", "NewArrival", "OperationSystem", "Price", "RearCamera", "ReleaseDate", "Views", "Weight" },
                values: new object[] { 2, null, "Li-Ion 5000 mAh, non-removable", false, "163.3 x 77.9 x 8.9 mm (6.43 x 3.07 x 0.35 in)", 1, 5, "Exynos 2200 (4 nm) - Europe", 0, null, null, "The Samsung Galaxy S22 Ultra is the headliner of the S22 series. It's the first S series phone to include Samsung's S Pen. Specifications are top-notch including 6.8-inch Dynamic AMOLED display with 120Hz refresh rate, Snapdragon 8 Gen 1 processor, 5000mAh battery, up to 12gigs of RAM, and 1TB of storage. In the camera department, a quad-camera setup is presented with two telephoto sensors.", null, null, "Dynamic AMOLED 2X, 120Hz, HDR10+, 1750 nits (peak)", "40 MP, f/2.2, 26mm (wide), 1/2.82\", 0.7µm, PDAF", "Xclipse 920 - Europe", false, false, false, null, "128GB 8GB RAM, 256GB 12GB RAM, 512GB 12GB RAM, 1TB 12GB RAM", "Samsung Galaxy S22 Ultra", false, "Android", 2000.0, "108 MP, f/1.8, 23mm (wide), 1/1.33\", 0.8µm, PDAF, Laser AF, OIS\r\n10 MP, f/4.9, 230mm (periscope telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 10x optical zoom\r\n10 MP, f/2.4, 70mm (telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 3x optical zoom\r\n12 MP, f/2.2, 13mm, 120˚ (ultrawide), 1/2.55\", 1.4µm, dual pixel PDAF, Super Steady video", new DateTime(2021, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "228 g / 229 g (mmWave) (8.04 oz)" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AppUserId", "Battery", "Bestseller", "Body", "BrandId", "CategoryId", "Chipset", "Count", "CreatedTime", "DeletedAt", "Description", "DiscountPercent", "DiscountPrice", "Display", "FrontCamera", "GPU", "InStock", "IsDeleted", "IsFeatured", "LastUpdatedAt", "Memory", "Name", "NewArrival", "OperationSystem", "Price", "RearCamera", "ReleaseDate", "Views", "Weight" },
                values: new object[] { 3, null, "Li-Ion, non-removable", false, "160.7 x 77.6 x 7.9 mm (6.33 x 3.06 x 0.31 in)", 2, 5, "Apple A16 Bionic (5 nm)", 0, null, null, "The Samsung Galaxy S22 Ultra is the headliner of the S22 series. It's the first S series phone to include Samsung's S Pen. Specifications are top-notch including 6.8-inch Dynamic AMOLED display with 120Hz refresh rate, Snapdragon 8 Gen 1 processor, 5000mAh battery, up to 12gigs of RAM, and 1TB of storage. In the camera department, a quad-camera setup is presented with two telephoto sensors.", null, null, "Super Retina XDR OLED, 120Hz, HDR10, Dolby Vision, 1000 nits (HBM), 1200 nits (peak)", "12 MP, f/2.2, 23mm (wide), 1/3.6\", 4K@24/25/30/60fps, 1080p@30/60/120fps, gyro-EIS\r\n", "Apple GPU", false, false, false, null, "128GB 6GB RAM, 256GB 6GB RAM, 512GB 6GB RAM, 1TB 6GB RAM", "Apple iPhone 14 Pro Max", false, "IOS", 1500.0, "48 MP, (wide), dual pixel PDAF, sensor-shift OIS\r\n12 MP, f/2.8, 77mm (telephoto), PDAF, OIS, 3x optical zoom\r\n12 MP, f/1.8, 13mm, 120˚ (ultrawide), 1.4µm, PDAF\r\nTOF 3D LiDAR scanner (depth)", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "228 g / 229 g (mmWave) (8.04 oz)" });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "IsMain", "ProductId" },
                values: new object[,]
                {
                    { 1, "poco-f1.jpg", true, 1 },
                    { 2, "poco-f1.jpg", false, 1 },
                    { 3, "s22.jpg", true, 2 },
                    { 4, "s22-2.jpg", false, 2 },
                    { 5, "14max.jpg", true, 3 },
                    { 6, "14max-2.jpg", false, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "GPU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: null);
        }
    }
}
