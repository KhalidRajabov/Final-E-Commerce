using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entities
{
    public class Product:BaseEntity
    {
        public double Price { get; set; }
        
        public string? Description { get; set; }
        public Nullable<DateTime> ReleaseDate { get; set; }
        public string? GPU { get; set; }
        public string? Body { get; set; }
        public string? Chipset { get; set; }
        public string? Display { get; set; }
        public string? OperationSystem { get; set; }
        public string? Memory { get; set; }
        public string? FrontCamera { get; set; }
        public string? RearCamera { get; set; }
        public string? Battery { get; set; }
        public string? Weight { get; set; }

        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        
        public int Count { get; set; }
        public int Views { get; set; }
        public bool IsFeatured { get; set; }
        public bool Bestseller { get; set; }
        public bool NewArrival { get; set; }
        public bool InStock { get; set; }


        [NotMapped]
        public List<IFormFile>? Photo { get; set; }

        [NotMapped]
        public List<int>? TagId { get; set; }




        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }




        public List<ProductTag>? ProductTags { get; set; }

        public List<ProductImage>? ProductImages { get; set; }

        public List<OrderItem>? OrderItem { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
    }
}