using Final_E_Commerce.Entity;
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
        public DbSet<CustomBrand>? CustomBrands { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductImage>? ProductImages { get; set; }
        public DbSet<ProductTag>? ProductTags { get; set; }
        public DbSet<Slider>? Sliders { get; set; }
        public DbSet<Subscriber>? Subscribers { get; set; }
        public DbSet<Tag>? Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
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
               });
        }
    }
}