using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entity
{
    public class Product:BaseEntity
    {
        public double Price { get; set; }
        
        public string? Description { get; set; }
        
        public Nullable<double> DiscountPercent { get; set; }
        
        public Nullable<double> DiscountPrice { get; set; }
        
        public int Count { get; set; }
        public bool IsFeatured { get; set; }
        public bool Bestseller { get; set; }
        public bool NewArrival { get; set; }
        public bool InStock { get; set; }


        [NotMapped]
        public List<IFormFile>? Photo { get; set; }

        [NotMapped]
        public List<int>? TagId { get; set; }


        



        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        public int CustomBrandId { get; set; }
        public CustomBrand? CustomBrand { get; set; }


        public List<UserProduct>? UserProducts { get; set; }
        public List<ProductTag>? ProductTags { get; set; }

        public List<ProductImage>? ProductImages { get; set; }

        public List<OrderItem>? OrderItem { get; set; }
    }
}
