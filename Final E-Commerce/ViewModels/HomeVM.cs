using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class HomeVM
    {
        public Bio? Bio { get; set; }
        public AppUser? User { get; set; }
        public Category? Category { get; set; }
        public Products? MostPopularProduct { get; set; }
        public List<Products>? PopularProducts { get; set; }
        public List<Products>? BestSellerProducts { get; set; }
        public List<Products>? Following { get; set; }
        public List<Slider>? Sliders { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
    }
}
