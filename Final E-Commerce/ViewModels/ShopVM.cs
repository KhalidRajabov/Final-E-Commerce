using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final_E_Commerce.ViewModels
{
    public class ShopVM
    {
        public string? Search { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public string? AlphabeticOrder { get; set; }
        public string? DateOrder { get; set; }
        public string? Speciality { get; set; }
        public string? Price { get; set; }


        public AppUser? User { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
        public List<Products>? Products{ get; set; }
    }
}
