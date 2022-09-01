using Final_E_Commerce.Entities;

namespace WebApi.DTO.Product_DTOs
{
    public class ProductReturnDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public Nullable<DateTime> ReleaseDate { get; set; }
        public string? Description { get; set; }
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
        public bool Instock { get; set; }
        public int Views { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public IList<ProductImage>? ProductImage { get; set; }
        public ProductCategoryDTO? Category { get; set; }
        
    }

    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}