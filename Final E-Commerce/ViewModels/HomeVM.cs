using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class HomeVM
    {
        public Bio? Bio { get; set; }
        public Category? Category { get; set; }
        public List<Product>? PopularProducts { get; set; }
        public Product? MostPopularProduct { get; set; }
        public List<Product>? BestSellerProducts { get; set; }
        public List<Slider>? Sliders { get; set; }
        public AppUser? User { get; set; }
    }
}
