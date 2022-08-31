using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
                        
        public DbSet<AppUser>? AppUser { get; set; }
        public DbSet<Bio>? Bios { get; set; }
        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductImage>? ProductImages { get; set; }
        public DbSet<ProductTag>? ProductTags { get; set; }
        public DbSet<Slider>? Sliders { get; set; }
        public DbSet<Subscriber>? Subscribers { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Wishlist>? Wishlists { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region Roles (updates on every migration, use only for once)
            /*builder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Id = "39792324-9bcf-41cc-aff7-9421ab090dbf",
                   Name = "Member"
               },
               new IdentityRole
               {
                   Id = "d78fa29e-8b9b-431d-90e1-312c634436da",
                   Name = "Editor"
               },
               new IdentityRole
               {
                   Id = "64830485-9bcf-41fr-aff7-3333ab090dbf",
                   Name = "Moderator"
               },
               new IdentityRole
               {
                   Id = "7985400a-d644-4954-a0c5-f579a46dd5c6",
                   Name = "Admin"
               },
               new IdentityRole
               {
                   Id = "d76fa29e-8b9b-431d-90e1-641c634654da",
                   Name = "SuperAdmin"
               });*/
            #endregion
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Phones and Tablets"
                },
                new Category
                {
                    Id=2,
                    Name = "Watches"
                },
                new Category
                {
                    Id=3,
                    Name="TV, Audio and Video"
                },
                new Category
                {
                    Id=4,
                    Name="Computers and accessories"
                },
                new Category
                {
                    Id=5,
                    Name="Smartphones",
                    ParentId=1
                },
                new Category
                {
                    Id=6,
                    Name="Tablets",
                    ParentId = 1
                },
                new Category
                {
                    Id=7,
                    Name="Mobile Accessories",
                    ParentId=1
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
                    Id=1,
                    Logo="logo.png",
                    Headertext= "Free shipping • Free 30 days return • Express delivery",
                    Address="Bakı şəhəri, Səbayıl rayonu, G.Əliyev küç.54D",
                    Phone="+994515186423",
                    Email="khalidsr@code.edu.az",
                    Author="Khalid Rajabov",
                    Facebook= "https://www.facebook.com/khalid.radjabov.5",
                    Linkedin= "https://www.linkedin.com/mwlite/in/khalid-racabov-867281243",
                    Instagram= "https://www.instagram.com/mr.felix_666/",
                    Twitter= "https://twitter.com/slayer_dante_?t=G-VmlfAok6IC_vxmQnYfLQ&s=08",
                    NewsLetterHeader= "Our Newsletter",
                    NewsLetterText= "Subscribe to our Newsletter to receive early discount offers"
                }
                );
        }
        
    }
}