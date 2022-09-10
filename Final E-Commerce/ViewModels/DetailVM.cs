using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class DetailVM
    {
        public Product? Product { get; set; }
        public List<Product>? RelatedProducts { get; set; }
        public List<Product> ListProducts { get; set; }
    }
}
