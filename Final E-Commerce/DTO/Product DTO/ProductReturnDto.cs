using System;

namespace WebApi.DTO.Product_DTOs
{
    public class ProductReturnDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool Instock { get; set; }
        public bool IsFeatured { get; set; }
        public bool Bestseller { get; set; }
        public bool NewArrival { get; set; }
        public bool InStock { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public ProductCategoryDTO? Category { get; set; }
    }

    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
