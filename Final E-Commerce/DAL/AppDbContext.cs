using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>? options):base(options)
        {
        }
                        
        public DbSet<AppUser>? AppUser { get; set; }
        public DbSet<Bio>? Bios { get; set; }
        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<ProductImage>? ProductImages { get; set; }
        public DbSet<ProductTag>? ProductTags { get; set; }
        public DbSet<Slider>? Sliders { get; set; }
        public DbSet<Subscriber>? Subscribers { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Wishlist>? Wishlists { get; set; }
        public DbSet<UserProfile>? UserProfiles { get; set; }
        public DbSet<UserDetails>? UserDetails { get; set; }
        public DbSet<Blogs>? Blogs { get; set; }
        public DbSet<BlogSubject>? BlogSubjects  { get; set; }
        public DbSet<Subjects>? Subjects { get; set; }
        public DbSet<BlogComment>? BlogComments { get; set; }
        public DbSet<ProductComment>? ProductComments{ get; set; }
        public DbSet<UserProductRatings>? UserProductRatings { get; set; }
        public DbSet<Messages>? Messages { get; set; }
        public DbSet<UserSubscription>? Subscription { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Phones and Tablets",
                    ImageUrl = "tabletsandphones.jpg"
                },
                new Category
                {
                    Id = 2,
                    Name = "Watches",
                    ImageUrl = "product-sm-8.jpg"
                },
                new Category
                {
                    Id = 3,
                    Name = "TV, Audio and Video",
                    ImageUrl = "tv.jpg"
                },
                new Category
                {
                    Id = 4,
                    Name = "Computers and accessories",
                    ImageUrl = "computers.jpg"
                },
                new Category
                {
                    Id = 5,
                    Name = "Smartphones",
                    ParentId = 1
                },
                new Category
                {
                    Id = 6,
                    Name = "Tablets",
                    ParentId = 1
                },
                new Category
                {
                    Id = 7,
                    Name = "Mobile Accessories",
                    ParentId = 1
                },
                new Category
                {
                    Id = 8,
                    Name = "Smart watches",
                    ParentId = 2
                },
                new Category
                {
                    Id = 9,
                    Name = "Analog watches",
                    ParentId = 2
                },
                new Category
                {
                    Id = 10,
                    Name = "Kids watch",
                    ParentId = 2
                },
                new Category
                {
                    Id = 11,
                    Name = "Watch Accessories",
                    ParentId = 2
                },
                new Category
                {
                    Id = 12,
                    Name = "TVs",
                    ParentId = 3
                },
                new Category
                {
                    Id = 13,
                    Name = "TV/Audio Video Accessories",
                    ParentId = 3
                });
            builder.Entity<Bio>().HasData(
                new Bio
                {
                    Id = 1,
                    Logo = "logo.png",
                    Headertext = "Free shipping • Free 30 days return • Express delivery",
                    Address = "Bakı şəhəri, Səbayıl rayonu, G.Əliyev küç.54D",
                    Phone = "+994515186423",
                    Email = "khalidsr@code.edu.az",
                    Author = "Khalid Rajabov",
                    Facebook = "https://www.facebook.com/khalid.radjabov.5",
                    Linkedin = "https://www.linkedin.com/mwlite/in/khalid-racabov-867281243",
                    Instagram = "https://www.instagram.com/mr.felix_666/",
                    Twitter = "https://twitter.com/slayer_dante_?t=G-VmlfAok6IC_vxmQnYfLQ&s=08",
                    NewsLetterHeader = "Our Newsletter",
                    NewsLetterText = "Subscribe to our Newsletter to receive early discount offers"
                }
                );
            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 1,
                    Name = "Android",
                    ImageUrl = "android.jpg"
                },
                new Brand
                {
                    Id = 2,
                    Name = "Apple",
                    ImageUrl = "apple.jpg"
                }
                );
            builder.Entity<Products>().HasData(
                new Products
                {
                    Id = 1,
                    BrandId = 1,
                    CategoryId = 5,
                    Price = 750,
                    Name = "Xiaomi Pocophone F1",
                    ReleaseDate = new DateTime(2018, 8, 8),
                    Description = "POCO F1 (Rosso Red, 128 GB) (6 GB RAM) Meet the POCO F1 - the first flagship smartphone from POCO by Xiaomi. The POCO F1 sports Qualcomm flagship Snapdragon 845 processor, an octa-core CPU with a maximum clock speed of 2.8 GHz which is supported by 6 GB of LPDDR4X RAM.",
                    Body = "155.5 x 75.3 x 8.8 mm (6.12 x 2.96 x 0.35 in)",
                    Weight = "182 g (6.42 oz)",
                    OperationSystem = "Android 8.1 (Oreo), upgradable to Android 10, MIUI 12",
                    GPU = "Adreno 630",
                    Chipset = "Qualcomm SDM845 Snapdragon 845 (10 nm)",
                    Memory = "64GB 6GB RAM, 128GB 6GB RAM, 256GB 8GB RAM",
                    Display = "6.18 inches, 96.2 cm2 (~82.2% screen-to-body ratio),1080 x 2246 pixels, 18.7:9 ratio (~403 ppi density)",
                    RearCamera = "12 MP, f/1.9, 1/2.55, 1.4µm, dual pixel PDAF, 4K@30/60fps, 1080p@30fps (gyro-EIS), 1080p@240fps, 720p@960fps",
                    FrontCamera = "20 MP, f/2.0, (wide), 1/3, 0.9µm, 1080p@30fps",
                    Battery = "Li-Po 4000 mAh, non-removable, quick charge 3.0",
                    Count = 50,
                    Sold=0,
                    Profit=0,
                    Views=0,
                    Status=ProductConfirmationStatus.Approved
                },
                new Products
                {
                    Id = 2,
                    BrandId = 1,
                    CategoryId = 5,
                    Price = 2000,
                    DiscountPercent = 5,
                    DiscountPrice = 1900,
                    DiscountUntil = new DateTime(2023, 12, 12),
                    Name = "Samsung Galaxy S22 Ultra",
                    ReleaseDate = new DateTime(2021, 8, 8),
                    Description = "The Samsung Galaxy S22 Ultra is the headliner of the S22 series. It's the first S series phone to include Samsung's S Pen. Specifications are top-notch including 6.8-inch Dynamic AMOLED display with 120Hz refresh rate, Snapdragon 8 Gen 1 processor, 5000mAh battery, up to 12gigs of RAM, and 1TB of storage. In the camera department, a quad-camera setup is presented with two telephoto sensors.",
                    Body = "163.3 x 77.9 x 8.9 mm (6.43 x 3.07 x 0.35 in)",
                    Weight = "228 g / 229 g (mmWave) (8.04 oz)",
                    OperationSystem = "Android",
                    Chipset = "Exynos 2200 (4 nm) - Europe",
                    Memory = "128GB 8GB RAM, 256GB 12GB RAM, 512GB 12GB RAM, 1TB 12GB RAM",
                    GPU = "Xclipse 920 - Europe",
                    Display = "Dynamic AMOLED 2X, 120Hz, HDR10+, 1750 nits (peak)",
                    RearCamera = "108 MP, f/1.8, 23mm (wide), 1/1.33, 0.8µm, PDAF, Laser AF, OIS 10 MP, f/4.9, 230mm (periscope telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 10x optical zoom\r\n10 MP, f/2.4, 70mm (telephoto), 1/3.52\", 1.12µm, dual pixel PDAF, OIS, 3x optical zoom 12 MP, f/2.2, 13mm, 120˚ (ultrawide), 1/2.55\", 1.4µm, dual pixel PDAF, Super Steady video",
                    FrontCamera = "40 MP, f/2.2, 26mm (wide), 1/2.82, 0.7µm, PDAF",
                    Battery = "Li-Ion 5000 mAh, non-removable",
                    Count = 50,
                    Sold = 0,
                    Profit = 0,
                    Views = 0,
                    Status = ProductConfirmationStatus.Approved
                },
                new Products
                {
                    Id = 3,
                    BrandId = 2,
                    CategoryId = 5,
                    Price = 1500,
                    DiscountPercent = 25,
                    DiscountPrice = 1125,
                    DiscountUntil = new DateTime(2022, 11, 12),
                    Name = "Apple iPhone 14 Pro Max",
                    ReleaseDate = new DateTime(2022, 9, 1),
                    Description = "The Samsung Galaxy S22 Ultra is the headliner of the S22 series. It's the first S series phone to include Samsung's S Pen. Specifications are top-notch including 6.8-inch Dynamic AMOLED display with 120Hz refresh rate, Snapdragon 8 Gen 1 processor, 5000mAh battery, up to 12gigs of RAM, and 1TB of storage. In the camera department, a quad-camera setup is presented with two telephoto sensors.",
                    Body = "160.7 x 77.6 x 7.9 mm (6.33 x 3.06 x 0.31 in)",
                    Weight = "228 g / 229 g (mmWave) (8.04 oz)",
                    OperationSystem = "IOS",
                    Chipset = "Apple A16 Bionic (5 nm)",
                    Memory = "128GB 6GB RAM, 256GB 6GB RAM, 512GB 6GB RAM, 1TB 6GB RAM",
                    GPU = "Apple GPU",
                    Display = "Super Retina XDR OLED, 120Hz, HDR10, Dolby Vision, 1000 nits (HBM), 1200 nits (peak)",
                    RearCamera = "48 MP, (wide), dual pixel PDAF, sensor-shift OIS 12 MP, f/2.8, 77mm (telephoto), PDAF, OIS, 3x optical zoom 12 MP, f/1.8, 13mm, 120˚ (ultrawide), 1.4µm, PDAF TOF 3D LiDAR scanner (depth)",
                    FrontCamera = "12 MP, f/2.2, 23mm (wide), 1/3.6, 4K@24/25/30/60fps, 1080p@30/60/120fps, gyro-EIS",
                    Battery = "Li-Ion, non-removable",
                    Count = 50,
                    Sold = 0,
                    Profit = 0,
                    Views = 0,
                    Status = ProductConfirmationStatus.Approved
                }
                );
            builder.Entity<ProductImage>().HasData(
                new ProductImage
                {
                    Id = 1,
                    ProductId = 1,
                    ImageUrl = "poco-f1.jpg",
                    IsMain = true,
                },
                new ProductImage
                {
                    Id = 2,
                    ProductId = 1,
                    ImageUrl = "poco-f1-2.jpg",
                },
                new ProductImage
                {
                    Id = 3,
                    ProductId = 2,
                    ImageUrl = "s22.jpg",
                    IsMain = true,
                },
                new ProductImage
                {
                    Id = 4,
                    ProductId = 2,
                    ImageUrl = "s22-2.jpg",
                },
                new ProductImage
                {
                    Id = 5,
                    ProductId = 3,
                    ImageUrl = "14max.jpg",
                    IsMain = true,
                },
                new ProductImage
                {
                    Id = 6,
                    ProductId = 3,
                    ImageUrl = "14max-2.jpg",
                },
                new ProductImage
                {
                    Id = 7,
                    ProductId = 1,
                    ImageUrl = "Xiaomi-Pocophone-F1-Steel-Blue.jpg",
                },
                new ProductImage
                {
                    Id = 8,
                    ProductId = 1,
                    ImageUrl = "Xiaomi-Pocophone-F1-launch-1.jpg",
                },
                new ProductImage
                {
                    Id = 9,
                    ProductId = 2,
                    ImageUrl = "Samsung-Galaxy-S22-family-in-blue-spread-like-cards-angled.jpg",
                },
                new ProductImage
                {
                    Id = 10,
                    ProductId = 2,
                    ImageUrl = "Samsung_GalaxyS22Ultra_LEAD.jpg",
                },
                new ProductImage
                {
                    Id = 11,
                    ProductId = 3,
                    ImageUrl = "gsmarena_000.jpg",
                },
                new ProductImage
                {
                    Id = 12,
                    ProductId = 3,
                    ImageUrl = "32ff69fafded8e6b986bc76f410e0ce5.jpg",
                }
                );
            builder.Entity<Tag>().HasData(
                new Tag
                {
                    Id = 1,
                    Name = "flagman"
                },
                new Tag
                {
                    Id = 2,
                    Name = "gaming"
                },
                new Tag
                {
                    Id = 3,
                    Name = "Office"
                },
                new Tag
                {
                    Id = 4,
                    Name = "Budget"
                }
                );
        }

    }
}